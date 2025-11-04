using LigaLibre.Application.DTOs;
using LigaLibre.Domain.Enums;

namespace LigaLibre.Tests.DTOs;

/// <summary>
/// Pruebas unitarias para RefereeDto
/// </summary>
public class RefereeDtoTests
{
    [Fact]
    public void RefereeDto_DefaultValues_AreCorrect()
    {
        var dto = new RefereeDto();

        Assert.Equal(0, dto.Id);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.LicenseNumber);
        Assert.False(dto.IsActive);
    }

    [Fact]
    public void RefereeDto_SetProperties_WorksCorrectly()
    {
        var dto = new RefereeDto
        {
            Id = 1,
            FirstName = "Juan",
            LastName = "Perez",
            LicenseNumber = "REF001",
            IsActive = true,
            Category = RefereeCategoryEnum.National
        };

        Assert.Equal(1, dto.Id);
        Assert.Equal("Juan", dto.FirstName);
        Assert.Equal("Perez", dto.LastName);
        Assert.Equal("REF001", dto.LicenseNumber);
        Assert.True(dto.IsActive);
        Assert.Equal(RefereeCategoryEnum.National, dto.Category);
    }
}

/// <summary>
/// Pruebas unitarias para CreateRefereeDto
/// </summary>
public class CreateRefereeDtoTests
{
    [Fact]
    public void CreateRefereeDto_SetProperties_WorksCorrectly()
    {
        var dto = new CreateRefereeDto
        {
            FirstName = "Carlos",
            LastName = "Lopez",
            LicenseNumber = "REF002",
            IsActive = true,
            Category = RefereeCategoryEnum.International
        };

        Assert.Equal("Carlos", dto.FirstName);
        Assert.Equal("Lopez", dto.LastName);
        Assert.Equal("REF002", dto.LicenseNumber);
        Assert.True(dto.IsActive);
        Assert.Equal(RefereeCategoryEnum.International, dto.Category);
    }
}

/// <summary>
/// Pruebas unitarias para UpdateRefereeDto
/// </summary>
public class UpdateRefereeDtoTests
{
    [Fact]
    public void UpdateRefereeDto_SetProperties_WorksCorrectly()
    {
        var dto = new UpdateRefereeDto
        {
            Id = 1,
            FirstName = "Pedro",
            LastName = "Garcia",
            LicenseNumber = "REF003",
            IsActive = false,
            Category = RefereeCategoryEnum.Regional
        };

        Assert.Equal(1, dto.Id);
        Assert.Equal("Pedro", dto.FirstName);
        Assert.Equal("Garcia", dto.LastName);
        Assert.Equal("REF003", dto.LicenseNumber);
        Assert.False(dto.IsActive);
        Assert.Equal(RefereeCategoryEnum.Regional, dto.Category);
    }
}
