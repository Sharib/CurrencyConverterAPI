using CurrencyConverterAPI.Application.Common.Interfaces;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Queries.ListIncidents;

public class ListIncidentsQueryHandler(IMaintenanceIncidentRepository repository)
    : IRequestHandler<ListIncidentsQuery, ListIncidentsResult>
{
    public async Task<ListIncidentsResult> Handle(ListIncidentsQuery request, CancellationToken cancellationToken)
    {
        var incidents = request.Status.HasValue
            ? await repository.GetByStatusAsync(request.Status.Value)
            : await repository.GetAllAsync();

        return new ListIncidentsResult(incidents);
    }
}
