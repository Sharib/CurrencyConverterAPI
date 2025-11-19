namespace CurrencyConverterAPI.Application.MaintenanceIncident.Queries.GetIncident;

public record GetIncidentQuery(Guid IncidentId) : IRequest<GetIncidentResult>;
