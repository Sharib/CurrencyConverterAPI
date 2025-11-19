using CurrencyConverterAPI.Application.Common.Interfaces;
using CurrencyConverterAPI.Domain.Entities;
using CurrencyConverterAPI.Domain.Enums;
using CurrencyConverterAPI.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CurrencyConverterAPI.Infrastructure.Repositories;

public class MongoDbMaintenanceIncidentRepository : IMaintenanceIncidentRepository
{
    private readonly IMongoCollection<MaintenanceIncident> _incidents;

    public MongoDbMaintenanceIncidentRepository(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
        _incidents = mongoDatabase.GetCollection<MaintenanceIncident>(settings.Value.IncidentsCollectionName);
    }

    public async Task<MaintenanceIncident> CreateAsync(MaintenanceIncident incident)
    {
        await _incidents.InsertOneAsync(incident);
        return incident;
    }

    public async Task<MaintenanceIncident?> GetByIdAsync(Guid id)
    {
        var filter = Builders<MaintenanceIncident>.Filter.Eq(i => i.Id, id);
        return await _incidents.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MaintenanceIncident>> GetAllAsync()
    {
        return await _incidents.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<MaintenanceIncident>> GetByStatusAsync(IncidentStatus status)
    {
        var filter = Builders<MaintenanceIncident>.Filter.Eq(i => i.Status, status);
        return await _incidents.Find(filter).ToListAsync();
    }

    public async Task<MaintenanceIncident> UpdateAsync(MaintenanceIncident incident)
    {
        var filter = Builders<MaintenanceIncident>.Filter.Eq(i => i.Id, incident.Id);
        await _incidents.ReplaceOneAsync(filter, incident);
        return incident;
    }
}
