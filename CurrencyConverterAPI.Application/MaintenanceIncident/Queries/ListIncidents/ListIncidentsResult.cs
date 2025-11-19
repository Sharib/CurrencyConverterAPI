namespace CurrencyConverterAPI.Application.MaintenanceIncident.Queries.ListIncidents;

public record ListIncidentsResult(IEnumerable<Domain.Entities.MaintenanceIncident> Incidents);
