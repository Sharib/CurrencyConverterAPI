using CurrencyConverterAPI.Domain.Enums;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Commands.CreateIncident;

public record CreateIncidentCommand(
    string Description,
    IncidentType Type,
    string ServiceCentre,
    decimal Cost
) : IRequest<CreateIncidentResult>;
