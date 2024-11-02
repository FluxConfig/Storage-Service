using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories;

public class VaultConfigurationRepository : BaseRepository, IVaultConfigurationRepository
{
    public VaultConfigurationRepository(IOptions<MongoDbOptions> options, IMongoClient mongoClient) :
        base(mongoOptions: options,
            mongoClient: mongoClient)
    {
    }

    public async Task<ConfigurationDataEntity> LoadConfiguration(string serviceApiKey, string configurationTag,
        CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMicroseconds(5), cancellationToken);
        throw new NotImplementedException();
    }
}