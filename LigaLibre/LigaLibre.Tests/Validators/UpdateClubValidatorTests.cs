using FluentValidation.TestHelper;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;

namespace LigaLibre.Tests.Validators;

/// <summary>
/// Pruebas unitarias para UpdateClubValidator
/// </summary>
public class UpdateClubValidatorTests
{
    private readonly UpdateClubValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new UpdateClubDto { Name = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var dto = new UpdateClubDto { Email = "invalid_email" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.Email);
    }

    [Fact]
    public void Should_Have_Error_When_NumberOfPartners_Is_Zero()
    {
        var dto = new UpdateClubDto { NumberOfPartners = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(c => c.NumberOfPartners);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new UpdateClubDto
        {
            Id = 1,
            Name = "Boca Juniors",
            City = "Buenos Aires",
            Email = "info@boca.com",
            StadiumName = "La Bombonera",
            NumberOfPartners = 50000,
            Address = "Brandsen 805"
        };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
