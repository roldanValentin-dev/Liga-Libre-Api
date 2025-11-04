using FluentValidation;
using FluentValidation.Results;
using LigaLibre.API.Controllers;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LigaLibre.Tests.Controllers;

public class ClubControllerTests
{
    private readonly Mock<IClubService> _mockService;
    private readonly Mock<IValidator<CreateClubDto>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateClubDto>> _mockUpdateValidator;
    private readonly ClubController _controller;


    public ClubControllerTests()
    {
        _mockService = new Mock<IClubService>();
        _mockCreateValidator = new Mock<IValidator<CreateClubDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateClubDto>>();
        _controller = new ClubController(_mockService.Object, _mockCreateValidator.Object, _mockUpdateValidator.Object);
    }


    [Fact]
    public async Task GetById_ExistingId_ReturnsOK()
    {
        // Arrange preparacion
        var club = new ClubDto { Id = 1, Name = "River" };
        _mockService.Setup(s => s.GetClubByIdAsync(1)).ReturnsAsync(club);

        // Act acto
        var result = await _controller.GetById(1);

        // Assert verificacion
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ClubDto>(okResult.Value);
        Assert.Equal(club.Id, returnValue.Id);
    }
    [Fact]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        // Arrange
        _mockService.Setup(s => s.GetClubByIdAsync(999)).ReturnsAsync((ClubDto?)null);
        // Act
        var result = await _controller.GetById(999);
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ValidDto_ReturnsCreated()
    {
        // Arrange
        var createDto = new CreateClubDto { Name = "River" };
        var clubDto = new ClubDto { Id = 1, Name = "River" };
        
        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(new ValidationResult());

        _mockService.Setup(s => s.CreateClubAsync(createDto)).ReturnsAsync(clubDto);
        // Act
        var result = await _controller.CreateClub(createDto);

        // Assert
        var okCreatedResult = Assert.IsType<ObjectResult>(result);
        var CreatedResult = Assert.IsType<ClubDto>(okCreatedResult.Value);

        Assert.Equal(201,okCreatedResult.StatusCode);
        Assert.Equal(clubDto.Id, CreatedResult.Id);
    }

    [Fact]
    public async Task Create_InvalidDto_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateClubDto { Name = "" };
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Name", "Name is required")
        });
        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(validationResult);

        //Act
        var result = await _controller.CreateClub(createDto);

        //Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errors = Assert.IsType<List<ValidationFailure>>(badRequestResult.Value);
        Assert.Single(errors);
        Assert.Equal("Name", errors[0].PropertyName);
        Assert.Equal("Name is required", errors[0].ErrorMessage);
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var clubs = new List<ClubDto> { new() { Id = 1, Name = "River" } };
        _mockService.Setup(s => s.GetAllClubsAsync()).ReturnsAsync(clubs);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(clubs, okResult.Value);
    }

    [Fact]
    public async Task UpdateClub_ValidDto_ReturnsOk()
    {
        var updateDto = new UpdateClubDto { Id = 1, Name = "River Plate" };
        var club = new ClubDto { Id = 1, Name = "River Plate" };
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.UpdateClubAsync(1, updateDto)).ReturnsAsync(club);

        var result = await _controller.UpdateClub(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(club, okResult.Value);
    }

    [Fact]
    public async Task UpdateClub_InvalidDto_ReturnsBadRequest()
    {
        var updateDto = new UpdateClubDto();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("Name", "Required") });
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(validationResult);

        var result = await _controller.UpdateClub(1, updateDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteClub_ReturnsOk()
    {
        _mockService.Setup(s => s.DeleteClubAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteClub(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value!);
    }
}
