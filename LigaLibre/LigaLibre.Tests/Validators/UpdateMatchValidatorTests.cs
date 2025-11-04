using FluentValidation.TestHelper;
using LigaLibre.Application.DTOs;
using LigaLibre.Application.Validators;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Tests.Validators;

/// <summary>
/// Pruebas unitarias para UpdateMatchValidator
/// </summary>
public class UpdateMatchValidatorTests
{
    private readonly UpdateMatchValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Round_Is_Zero()
    {
        var dto = new UpdateMatchDto { Round = 0 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.Round);
    }

    [Fact]
    public void Should_Have_Error_When_HomeClubId_Equals_AwayClubId()
    {
        var dto = new UpdateMatchDto { HomeClubId = 1, AwayClubId = 1 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.AwayClubId);
    }

    [Fact]
    public void Should_Have_Error_When_HomeScore_Is_Negative()
    {
        var dto = new UpdateMatchDto { HomeScore = -1 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.HomeScore);
    }

    [Fact]
    public void Should_Have_Error_When_AwayScore_Is_Negative()
    {
        var dto = new UpdateMatchDto { AwayScore = -1 };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(m => m.AwayScore);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var dto = new UpdateMatchDto
        {
            Round = 1,
            HomeClubId = 1,
            AwayClubId = 2,
            RefereeId = 1,
            Stadium = "Monumental",
            MatchDate = DateTime.Now,
            HomeScore = 2,
            AwayScore = 1,
            Status = MatchStatusEnum.Finished
        };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
