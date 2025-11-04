using FluentValidation;
using FluentValidation.Results;
using LigaLibre.API.Controllers;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Interfaces;
using LigaLibre.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LigaLibre.Tests.Controllers;

/// <summary>
/// Pruebas unitarias para RefereeController
/// </summary>
public class RefereeControllerTests
{
    private readonly Mock<IRefereeServices> _mockService;
    private readonly Mock<IValidator<CreateRefereeDto>> _mockCreateValidator;
    private readonly Mock<IValidator<UpdateRefereeDto>> _mockUpdateValidator;
    private readonly RefereeController _controller;

    public RefereeControllerTests()
    {
        _mockService = new Mock<IRefereeServices>();
        _mockCreateValidator = new Mock<IValidator<CreateRefereeDto>>();
        _mockUpdateValidator = new Mock<IValidator<UpdateRefereeDto>>();
        _controller = new RefereeController(_mockService.Object, _mockCreateValidator.Object, _mockUpdateValidator.Object);
    }

    [Fact]
    public async Task GetAllReferees_ReturnsOk()
    {
        var referees = new List<RefereeDto> { new() { Id = 1, FirstName = "Juan", LastName = "Perez" } };
        _mockService.Setup(s => s.GetAllRefereeAsync()).ReturnsAsync(referees);

        var result = await _controller.GetAllReferees();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(referees, okResult.Value);
    }

    [Fact]
    public async Task GetActivesReferees_ReturnsOk()
    {
        var referees = new List<RefereeDto> { new() { Id = 1, FirstName = "Juan", LastName = "Perez", IsActive = true } };
        _mockService.Setup(s => s.GetActiveRefereeAsync()).ReturnsAsync(referees);

        var result = await _controller.GetActivesReferees();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(referees, okResult.Value);
    }

    [Fact]
    public async Task GetRefereeById_ExistingId_ReturnsOk()
    {
        var referee = new RefereeDto { Id = 1, FirstName = "Juan", LastName = "Perez" };
        _mockService.Setup(s => s.GetRefereeByIdAsync(1)).ReturnsAsync(referee);

        var result = await _controller.GetRefereeById(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(referee, okResult.Value);
    }

    [Fact]
    public async Task CreateReferee_ValidDto_ReturnsCreated()
    {
        var createDto = new CreateRefereeDto { FirstName = "Juan", LastName = "Perez", LicenseNumber = "REF001", Category = RefereeCategoryEnum.National };
        var referee = new RefereeDto { Id = 1, FirstName = "Juan", LastName = "Perez", LicenseNumber = "REF001" };
        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.CreateRefereeAsync(createDto)).ReturnsAsync(referee);

        var result = await _controller.CreateReferee(createDto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(referee, createdResult.Value);
        Assert.Equal(nameof(_controller.GetRefereeById), createdResult.ActionName);
    }

    [Fact]
    public async Task CreateReferee_InvalidDto_ReturnsBadRequest()
    {
        var createDto = new CreateRefereeDto();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("FirstName", "Required") });
        _mockCreateValidator.Setup(v => v.ValidateAsync(createDto, default)).ReturnsAsync(validationResult);

        var result = await _controller.CreateReferee(createDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateReferee_ValidDto_ReturnsOk()
    {
        var updateDto = new UpdateRefereeDto { Id = 1, FirstName = "Juan", LastName = "Perez", LicenseNumber = "REF001", Category = RefereeCategoryEnum.National };
        var referee = new RefereeDto { Id = 1, FirstName = "Juan", LastName = "Perez" };
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(new ValidationResult());
        _mockService.Setup(s => s.UpdateRefereeAsync(1, updateDto)).ReturnsAsync(referee);

        var result = await _controller.UpdateReferee(1, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(referee, okResult.Value);
    }

    [Fact]
    public async Task UpdateReferee_InvalidDto_ReturnsBadRequest()
    {
        var updateDto = new UpdateRefereeDto();
        var validationResult = new ValidationResult(new[] { new ValidationFailure("FirstName", "Required") });
        _mockUpdateValidator.Setup(v => v.ValidateAsync(updateDto, default)).ReturnsAsync(validationResult);

        var result = await _controller.UpdateReferee(1, updateDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteReferee_ExistingReferee_ReturnsNoContent()
    {
        _mockService.Setup(s => s.DeleteRefereeAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteReferee(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteReferee_NonExistingReferee_ReturnsNotFound()
    {
        _mockService.Setup(s => s.DeleteRefereeAsync(999)).ReturnsAsync(false);

        var result = await _controller.DeleteReferee(999);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("El arbitro con id:999 no encontrado", notFoundResult.Value);
    }
}
