using CurrencyConverterAPI.Domain.Enums;

namespace CurrencyConverterAPI.Application.Common.Interfaces;

public interface IMaintenanceIncidentRepository
{
    Task<Domain.Entities.MaintenanceIncident> CreateAsync(Domain.Entities.MaintenanceIncident incident);
    Task<Domain.Entities.MaintenanceIncident?> GetByIdAsync(Guid id);
    Task<IEnumerable<Domain.Entities.MaintenanceIncident>> GetAllAsync();
    Task<IEnumerable<Domain.Entities.MaintenanceIncident>> GetByStatusAsync(IncidentStatus status);
    Task<Domain.Entities.MaintenanceIncident> UpdateAsync(Domain.Entities.MaintenanceIncident incident);
}
