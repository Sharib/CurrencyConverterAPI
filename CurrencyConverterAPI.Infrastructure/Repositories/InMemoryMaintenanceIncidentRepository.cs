using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Domain.Entities;
using CurrencyConverterAPI.Domain.Enums;

namespace CurrencyConverterAPI.Infrastructure.Repositories;

public class InMemoryMaintenanceIncidentRepository : IMaintenanceIncidentRepository
{
    private readonly List<MaintenanceIncident> _incidents = new();
    private readonly object _lock = new();

    public Task<MaintenanceIncident> CreateAsync(MaintenanceIncident incident)
    {
        lock (_lock)
        {
            _incidents.Add(incident);
        }
        return Task.FromResult(incident);
    }

    public Task<MaintenanceIncident?> GetByIdAsync(Guid id)
    {
        lock (_lock)
        {
            var incident = _incidents.FirstOrDefault(i => i.Id == id);
            return Task.FromResult(incident);
        }
    }

    public Task<IEnumerable<MaintenanceIncident>> GetAllAsync()
    {
        lock (_lock)
        {
            return Task.FromResult<IEnumerable<MaintenanceIncident>>(_incidents.ToList());
        }
    }

    public Task<IEnumerable<MaintenanceIncident>> GetByStatusAsync(IncidentStatus status)
    {
        lock (_lock)
        {
            var incidents = _incidents.Where(i => i.Status == status).ToList();
            return Task.FromResult<IEnumerable<MaintenanceIncident>>(incidents);
        }
    }

    public Task<MaintenanceIncident> UpdateAsync(MaintenanceIncident incident)
    {
        lock (_lock)
        {
            var existing = _incidents.FirstOrDefault(i => i.Id == incident.Id);
            if (existing != null)
            {
                _incidents.Remove(existing);
                _incidents.Add(incident);
            }
        }
        return Task.FromResult(incident);
    }
}
