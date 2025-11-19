namespace CurrencyConverterAPI.Application.MaintenanceIncident.Commands.ApproveIncident;

public record ApproveIncidentCommand(
    Guid IncidentId,
    string ReviewedBy,
    string? ReviewNotes
) : IRequest<ApproveIncidentResult>;
