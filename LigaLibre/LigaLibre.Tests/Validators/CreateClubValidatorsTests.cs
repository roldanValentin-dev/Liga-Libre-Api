using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;
using FluentValidation.TestHelper;

namespace LigaLibre.Tests.Validators;

public class CreateClubValidatorsTests
{
    private readonly CreateClubValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new CreateClubDto { Name = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }
    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var dto = new CreateClubDto { Email = "invalid_email" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }
    [Fact]
    public void Should_Have_Error_When_NumberOfPartners_Is_Zero()
    {
        var dto = new CreateClubDto { NumberOfPartners = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.NumberOfPartners);
    }

    [Fact]
    public void Should_Not_Have_When_Error_Valid()
    {
        var dto = new CreateClubDto
        {
            Name = "Boca Juniors",
            City = "Buenos Aires",
            Email = "info@boca.com",
            StadiumName = "La Bombonera",
            NumberOfPartners = 50000,
            Address = "Brandsen805",
        };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}

