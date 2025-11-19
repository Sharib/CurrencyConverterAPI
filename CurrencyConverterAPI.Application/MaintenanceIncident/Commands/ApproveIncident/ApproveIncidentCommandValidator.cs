using FluentValidation;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Commands.ApproveIncident;

public class ApproveIncidentCommandValidator : AbstractValidator<ApproveIncidentCommand>
{
    public ApproveIncidentCommandValidator()
    {
        RuleFor(x => x.IncidentId)
            .NotEmpty()
            .WithMessage("Incident ID is required");

        RuleFor(x => x.ReviewedBy)
            .NotEmpty()
            .WithMessage("Reviewer name is required")
            .MaximumLength(200)
            .WithMessage("Reviewer name cannot exceed 200 characters");
    }
}
