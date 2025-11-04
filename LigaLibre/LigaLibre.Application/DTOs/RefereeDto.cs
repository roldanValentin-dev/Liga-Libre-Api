using LigaLibre.Domain.Enums;

namespace LigaLibre.Application.DTOs;

public class RefereeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public RefereeCategoryEnum Category { get; set; }

    public DateTime CreatedAT {  get; set; }= DateTime.UtcNow;
    public DateTime UpdatedAT { get; set;} = DateTime.UtcNow;
}
public class CreateRefereeDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    public RefereeCategoryEnum Category { get; set; }
}
public class UpdateRefereeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public RefereeCategoryEnum Category { get; set; }
}

