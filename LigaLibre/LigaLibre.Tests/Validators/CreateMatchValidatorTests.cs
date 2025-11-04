using FluentValidation.TestHelper;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;

namespace LigaLibre.Tests.Validators;

/// <summary>
/// Pruebas unitarias para CreateMatchValidator
/// </summary>
public class CreateMatchValidatorTests
{
    private readonly CreateMatchValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Round_Is_Zero()
    {
        var dto = new CreateMatchDto { Round = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.Round);
    }

    [Fact]
    public void Should_Have_Error_When_HomeClubId_Is_Zero()
    {
        var dto = new CreateMatchDto { HomeClubId = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.HomeClubId);
    }

    [Fact]
    public void Should_Have_Error_When_AwayClubId_Is_Zero()
    {
        var dto = new CreateMatchDto { AwayClubId = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.AwayClubId);
    }

    [Fact]
    public void Should_Have_Error_When_HomeClubId_Equals_AwayClubId()
    {
        var dto = new CreateMatchDto { HomeClubId = 1, AwayClubId = 1 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.AwayClubId);
    }

    [Fact]
    public void Should_Have_Error_When_Stadium_Is_Empty()
    {
        var dto = new CreateMatchDto { Stadium = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.Stadium);
    }

    [Fact]
    public void Should_Have_Error_When_MatchDate_Is_Past()
    {
        var dto = new CreateMatchDto { MatchDate = DateTime.Now.AddDays(-1) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.MatchDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new CreateMatchDto
        {
            Round = 1,
            HomeClubId = 1,
            AwayClubId = 2,
            Stadium = "Monumental",
            MatchDate = DateTime.Now.AddDays(1)
        };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
