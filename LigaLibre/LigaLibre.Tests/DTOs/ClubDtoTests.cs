using LigaLibre.Application.DTOs;

namespace LigaLibre.Tests.DTOs;

public class ClubDtoTests
{
    [Fact]
    public void ClubDto_DefaultValues_AreCorrect()
    {
        var dto = new ClubDto();

        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.Name);
        Assert.Equal(string.Empty, dto.City);
    }

    [Fact]
    public void ClubDto_SetProperties_WorksCorrectly()
    {
        var dto = new ClubDto
        {
            Id = 1,
            Name = "River Plate",
            City = "Buenos Aires",
            StadiumName = "Monumental",
            NumberOfPartners = 50000,
            Email = "info@river.com",
            Phone = "123456789",
            Address = "Av. Figueroa Alcorta"
        };

        Assert.Equal(1, dto.Id);
        Assert.Equal("River Plate", dto.Name);
        Assert.Equal("Buenos Aires", dto.City);
    }
}

public class CreateClubDtoTests
{
    [Fact]
    public void CreateClubDto_SetProperties_WorksCorrectly()
    {
        var dto = new CreateClubDto
        {
            Name = "Boca Juniors",
            City = "Buenos Aires",
            StadiumName = "La Bombonera",
            NumberOfPartners = 40000
        };

        Assert.Equal("Boca Juniors", dto.Name);
        Assert.Equal("La Bombonera", dto.StadiumName);
    }
}
