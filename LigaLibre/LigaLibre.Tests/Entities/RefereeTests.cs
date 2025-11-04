using LigaLibre.Domain.Entities;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Tests.Entities;

public class RefereeTests
{
    [Fact]
    public void Referee_DefaultValues_AreCorrect()
    {
        var referee = new Referee();

        Assert.Equal(string.Empty, referee.FirstName);
        Assert.Equal(string.Empty, referee.LastName);
        Assert.Equal(string.Empty, referee.LicenseNumber);
    }

    [Fact]
    public void Referee_SetProperties_WorksCorrectly()
    {
        var referee = new Referee
        {
            FirstName = "Pierluigi",
            LastName = "Collina",
            LicenseNumber = "REF-001",
            Category = RefereeCategoryEnum.International
        };

        Assert.Equal("Pierluigi", referee.FirstName);
        Assert.Equal("Collina", referee.LastName);
        Assert.Equal("REF-001", referee.LicenseNumber);
        Assert.Equal(RefereeCategoryEnum.International, referee.Category);
    }

    [Fact]
    public void Referee_Matches_InitializesEmpty()
    {
        var referee = new Referee();

        Assert.NotNull(referee.Matches);
        Assert.Empty(referee.Matches);
    }
}
