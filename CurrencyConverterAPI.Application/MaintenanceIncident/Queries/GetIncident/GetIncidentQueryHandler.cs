using CurrencyConverterAPI.Application.Common.Interfaces;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Queries.GetIncident;

public class GetIncidentQueryHandler(IMaintenanceIncidentRepository repository)
    : IRequestHandler<GetIncidentQuery, GetIncidentResult>
{
    public async Task<GetIncidentResult> Handle(GetIncidentQuery request, CancellationToken cancellationToken)
    {
        var incident = await repository.GetByIdAsync(request.IncidentId);
        return new GetIncidentResult(incident);
    }
}
