using FluentValidation;
using LigaLibre.Application.DTOs;

namespace LigaLibre.Application.Validators;

public class CreateClubValidator : AbstractValidator<CreateClubDto>
{
    public CreateClubValidator()
    {

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("El nombre del club es requerido")
            .MaximumLength(100).WithMessage("El nombre del club no puede exceder los 100 caracteres");

        RuleFor(c => c.City)
            .NotEmpty().WithMessage("La ciudad es requerida")
            .MaximumLength(50).WithMessage("La ciudad no puede exceder los 50 caracteres");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("El correo electrónico es requerido")
            .EmailAddress().WithMessage("Formato de email invalido")
            .MaximumLength(100).WithMessage("El correo electrónico no puede exceder los 100 caracteres");

        RuleFor(c => c.NumberOfPartners)
            .GreaterThan(0).WithMessage("El numero de socios tiene que ser mayor a 0");

        RuleFor(c => c.Phone)
            .MaximumLength(20)
            .WithMessage("El telefono no puede exceder 20 caracteres");

        RuleFor(c => c.StadiumName)
            .NotEmpty().WithMessage("El nombre del estadio es requerido")
            .MaximumLength(50).WithMessage("El nombre del estadio no puede exceder los 50 caractere");

        RuleFor(c => c.Address).NotEmpty().WithMessage("La direccion es requerida")
            .MaximumLength(50).WithMessage("La direccion no puede exceder los 50 caracteres");
    }
}

