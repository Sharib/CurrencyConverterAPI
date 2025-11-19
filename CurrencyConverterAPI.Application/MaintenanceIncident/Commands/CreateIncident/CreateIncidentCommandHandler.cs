using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Domain.Enums;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Commands.CreateIncident;

public class CreateIncidentCommandHandler(IMaintenanceIncidentRepository repository) 
    : IRequestHandler<CreateIncidentCommand, CreateIncidentResult>
{
    public async Task<CreateIncidentResult> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = new Domain.Entities.MaintenanceIncident
        {
            Id = Guid.NewGuid(),
            Description = request.Description,
            Type = request.Type,
            ServiceCentre = request.ServiceCentre,
            Cost = request.Cost,
            Status = IncidentStatus.Draft,
            CreatedDate = DateTime.UtcNow
        };

        await repository.CreateAsync(incident);

        return new CreateIncidentResult(incident.Id, "Incident created successfully");
    }
}
