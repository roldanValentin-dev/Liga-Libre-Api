using FluentValidation;
using LigaLibre.Application.DTOs;

namespace LigaLibre.Application.Validators;


public class CreateRefereeValidator : AbstractValidator<CreateRefereeDto>
{
    
    public CreateRefereeValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es requerido")
            .MaximumLength(50).WithMessage("El nombre no puede exceder los 50 caracteres");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es requerido")
            .MaximumLength(50).WithMessage("El apellido no puede exceder los 50 caracteres");
        
        RuleFor(x => x.LicenseNumber)
            .NotEmpty().WithMessage("El número de licencia es requerido")
            .MaximumLength(20).WithMessage("El número de licencia no puede exceder los 20 caracteres");
        
        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("La categoría del árbitro no es válida");
    }
}
