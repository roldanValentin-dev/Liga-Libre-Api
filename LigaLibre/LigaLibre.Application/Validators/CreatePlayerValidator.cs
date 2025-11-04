
using FluentValidation;
using LigaLibre.Application.DTOs;

namespace LigaLibre.Application.Validators;

public class CreatePlayerValidator : AbstractValidator<CreatePlayerDto>
{
    public CreatePlayerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres");
        RuleFor(x => x.Age)
            .GreaterThan(16).WithMessage("La edad minima es de 17 años")
            .LessThan(50).WithMessage("la edad maxima esde 50 años");
        RuleFor(x => x.Position)
            .NotEmpty().WithMessage("La posición es requerida")
            .Must(BeAValidPosition).WithMessage("Posicion no valida");
        RuleFor(x => x.JerseyNumber).GreaterThan(0).WithMessage("El numero de camiseta debe ser mayor a 0")
            .LessThanOrEqualTo(99).WithMessage("El numero de camiseta no puede ser mayor a 99");
        RuleFor(x => x.Weight)
            .GreaterThan(50).WithMessage("El peso minimo es de 50kg")
            .LessThan(80).WithMessage("El peso maximo es de 80kg");
        RuleFor(x=> x.Height)
            .GreaterThan(1.60m).WithMessage("La altura minima es de 1.60 metros")
            .LessThan(2.20m).WithMessage("La altura maxima es de 2.20 metros");
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Now.AddYears(-16)).WithMessage("Debe ser mayor a 16 años");

    }
    private static bool BeAValidPosition(string position)
    {
        var validPositions = new[] { "Portero", "Defensa", "Mediocampista", "Delantero" };
        return validPositions.Contains(position);
    }
}
