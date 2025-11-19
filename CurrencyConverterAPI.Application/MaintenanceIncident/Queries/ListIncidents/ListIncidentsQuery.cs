using CurrencyConverterAPI.Domain.Enums;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Queries.ListIncidents;

public record ListIncidentsQuery(IncidentStatus? Status = null) : IRequest<ListIncidentsResult>;
