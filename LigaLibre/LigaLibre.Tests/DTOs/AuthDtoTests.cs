using LigaLibre.Application.DTOs;

namespace LigaLibre.Tests.DTOs;

public class LoginDtoTests
{
    [Fact]
    public void LoginDto_DefaultValues_AreCorrect()
    {
        var dto = new LoginDto();

        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.Password);
    }

    [Fact]
    public void LoginDto_SetProperties_WorksCorrectly()
    {
        var dto = new LoginDto
        {
            Email = "test@test.com",
            Password = "Password123!"
        };

        Assert.Equal("test@test.com", dto.Email);
        Assert.Equal("Password123!", dto.Password);
    }
}

public class RegisterDtoTests
{
    [Fact]
    public void RegisterDto_DefaultValues_AreCorrect()
    {
        var dto = new RegisterDto();

        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.Equal(string.Empty, dto.Password);
    }

    [Fact]
    public void RegisterDto_SetProperties_WorksCorrectly()
    {
        var dto = new RegisterDto
        {
            Email = "new@test.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "Password123!"
        };

        Assert.Equal("new@test.com", dto.Email);
        Assert.Equal("John", dto.FirstName);
        Assert.Equal("Doe", dto.LastName);
        Assert.Equal("Password123!", dto.Password);
    }
}

public class AuthResponseDtoTests
{
    [Fact]
    public void AuthResponseDto_DefaultValues_AreCorrect()
    {
        var dto = new AuthResponseDto();

        Assert.Equal(string.Empty, dto.Token);
        Assert.Equal(string.Empty, dto.Email);
        Assert.Equal(string.Empty, dto.FirstName);
        Assert.Equal(string.Empty, dto.LastName);
        Assert.NotNull(dto.Roles);
        Assert.Empty(dto.Roles);
    }

    [Fact]
    public void AuthResponseDto_SetProperties_WorksCorrectly()
    {
        var dto = new AuthResponseDto
        {
            Token = "token123",
            Email = "user@test.com",
            FirstName = "Jane",
            LastName = "Smith",
            Roles = new List<string> { "User", "Admin" }
        };

        Assert.Equal("token123", dto.Token);
        Assert.Equal("user@test.com", dto.Email);
        Assert.Equal("Jane", dto.FirstName);
        Assert.Equal("Smith", dto.LastName);
        Assert.Equal(2, dto.Roles.Count);
        Assert.Contains("User", dto.Roles);
        Assert.Contains("Admin", dto.Roles);
    }
}
