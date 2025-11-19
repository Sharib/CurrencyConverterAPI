using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Domain.Enums;

namespace CurrencyConverterAPI.Application.MaintenanceIncident.Commands.ApproveIncident;

public class ApproveIncidentCommandHandler(IMaintenanceIncidentRepository repository)
    : IRequestHandler<ApproveIncidentCommand, ApproveIncidentResult>
{
    public async Task<ApproveIncidentResult> Handle(ApproveIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await repository.GetByIdAsync(request.IncidentId);
        
        if (incident == null)
        {
            return new ApproveIncidentResult(false, "Incident not found");
        }

        if (incident.Status == IncidentStatus.Approved)
        {
            return new ApproveIncidentResult(false, "Incident is already approved");
        }

        incident.Status = IncidentStatus.Approved;
        incident.ReviewedBy = request.ReviewedBy;
        incident.ReviewNotes = request.ReviewNotes;
        incident.ReviewedDate = DateTime.UtcNow;
        incident.LastModifiedDate = DateTime.UtcNow;

        await repository.UpdateAsync(incident);

        return new ApproveIncidentResult(true, "Incident approved successfully");
    }
}
