
using FluentValidation.TestHelper;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;

namespace LigaLibre.Tests.Validators;

public class CreatePlayerValidatorTests
{
    private readonly CreatePlayerValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new CreatePlayerDto{ FirstName = ""};
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(player => player.FirstName);
    }
    [Fact]
    public void Should_Have_Error_When_Age_Is_Too_Youg()
    {
        var dto = new CreatePlayerDto { Age = 15 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(player => player.Age);
    }
    [Fact]
    public void Should_Have_Error_When_Position_Is_Invalid()
    {
        var dto = new CreatePlayerDto { Position = "Invalid Position" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(player => player.Position);
    }
    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new CreatePlayerDto { 
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
