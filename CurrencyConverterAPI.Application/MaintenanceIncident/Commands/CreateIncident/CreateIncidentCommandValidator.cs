using FluentValidation;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Commands.CreateIncident;

public class CreateIncidentCommandValidator : AbstractValidator<CreateIncidentCommand>
{
    public CreateIncidentCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.ServiceCentre)
            .NotEmpty()
            .WithMessage("Service centre is required")
            .MaximumLength(200)
            .WithMessage("Service centre name cannot exceed 200 characters");

        RuleFor(x => x.Cost)
            .GreaterThan(0)
            .WithMessage("Cost must be greater than 0");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Invalid incident type");
    }
}
