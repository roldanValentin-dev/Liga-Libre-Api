using FluentValidation.TestHelper;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Tests.Validators;

/// <summary>
/// Pruebas unitarias para UpdateRefereeValidator
/// </summary>
public class UpdateRefereeValidatorTests
{
    private readonly UpdateRefereeValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var dto = new UpdateRefereeDto { FirstName = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_LastName_Is_Empty()
    {
        var dto = new UpdateRefereeDto { LastName = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.LastName);
    }

    [Fact]
    public void Should_Have_Error_When_LicenseNumber_Is_Empty()
    {
        var dto = new UpdateRefereeDto { LicenseNumber = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(r => r.LicenseNumber);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new UpdateRefereeDto
        {
            Id = 1,
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
