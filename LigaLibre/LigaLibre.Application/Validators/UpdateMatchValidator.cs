
using FluentValidation;
using LigaLibre.Application.DTOs;

namespace LigaLibre.Application.Validators;

public class UpdateMatchValidator : AbstractValidator<UpdateMatchDto>
{
    public UpdateMatchValidator()
    {
        RuleFor(x => x.Round)
            .GreaterThan(0).WithMessage("La jornada debe ser mayor a 0");
        
        RuleFor(x => x.HomeClubId)
            .GreaterThan(0).WithMessage("El Id del club local es requerido");
        
        RuleFor(x => x.AwayClubId)
            .GreaterThan(0).WithMessage("El Id del club visitante es requerido")
            .NotEqual(x => x.HomeClubId).WithMessage("El club visitante no puede ser el mismo que el club local");
        
        RuleFor(x => x.RefereeId)
            .GreaterThan(0).WithMessage("El Id del árbitro es requerido");
        
        RuleFor(x => x.Stadium)
            .NotEmpty().WithMessage("El estadio es requerido");
        
        RuleFor(x => x.MatchDate)
            .NotEmpty().WithMessage("La fecha del partido es requerida");
        
        RuleFor(x => x.HomeScore)
            .GreaterThanOrEqualTo(0).When(x => x.HomeScore.HasValue)
            .WithMessage("El marcador local no puede ser negativo");
        
        RuleFor(x => x.AwayScore)
            .GreaterThanOrEqualTo(0).When(x => x.AwayScore.HasValue)
            .WithMessage("El marcador visitante no puede ser negativo");
        
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("El estado del partido no es válido");
    }
}
