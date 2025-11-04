using FluentValidation.TestHelper;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Tests.Validators;

/// <summary>
/// Pruebas unitarias para CreateRefereeValidator
/// </summary>
public class CreateRefereeValidatorTests
{
    private readonly CreateRefereeValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var dto = new CreateRefereeDto { FirstName = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_FirstName_Exceeds_MaxLength()
    {
        var dto = new CreateRefereeDto { FirstName = new string('A', 51) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var dto = new CreateRefereeDto { LastName = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.LastName);
    }

    [Fact]
    public void Should_Have_Error_When_LicenseNumber_Is_Empty()
    {
        var dto = new CreateRefereeDto { LicenseNumber = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.LicenseNumber);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new CreateRefereeDto
        {
            FirstName = "Juan",
            LastName = "Perez",
            LicenseNumber = "REF001",
            Category = RefereeCategoryEnum.National,
            IsActive = true
        };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
