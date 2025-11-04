using FluentValidation.TestHelper;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;

namespace LigaLibre.Tests.Validators;

/// <summary>
/// Pruebas unitarias para UpdatePlayerValidator
/// </summary>
public class UpdatePlayerValidatorTests
{
    private readonly UpdatePlayerValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_FirstName_Is_Empty()
    {
        var dto = new UpdatePlayerDto { FirstName = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(p => p.FirstName);
    }

    [Fact]
    public void Should_Have_Error_When_Age_Is_Too_Young()
    {
        var dto = new UpdatePlayerDto { Age = 15 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(p => p.Age);
    }

    [Fact]
    public void Should_Have_Error_When_Position_Is_Invalid()
    {
        var dto = new UpdatePlayerDto { Position = "Invalid Position" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(p => p.Position);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new UpdatePlayerDto
        {
            Id = 1,
            FirstName = "Lionel",
            LastName = "Messi",
            Age = 35,
            Position = "Delantero",
            JerseyNumber = 10,
            Height = 1.70m,
            Weight = 60,
            Nationality = "Argentina",
            DateOfBirth = new DateTime(1987, 6, 24),
            ClubId = 1
        };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
