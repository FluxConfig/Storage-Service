using FluxConfig.Storage.Domain.Contracts.Dal.Containers;
using FluxConfig.Storage.Domain.Contracts.Dal.Entities;
using FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;
using FluxConfig.Storage.Domain.Exceptions.Infrastructure;
using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Repositories;

public class VaultConfigurationRepository : BaseRepository, IVaultConfigurationRepository
{
    public VaultConfigurationRepository(
        IOptionsSnapshot<MongoDbCollectionOptions> options,
        IMongoClient mongoClient) :
        base(
            collectionOptions: options.Get(MongoDbCollectionOptions.VaultTag),
            mongoClient: mongoClient)
    {
    }

    public async Task<ConfigurationDataEntity> LoadConfiguration(string configurationKey, string configurationTag,
        CancellationToken cancellationToken)
    {
        try
        {
            return await LoadConfigurationUnsafe(
                configurationKey: configurationKey,
                configurationTag: configurationTag,
                cancellationToken: cancellationToken
            );
        }
        catch (InvalidOperationException ex)
        {
            throw new EntityNotFoundException("Configuration data not found", configurationTag, ex);
        }
        catch (Exception ex)
        {
            throw new InfrastructureException("Unexpected exception occured during configurations.vault read", ex);
        }
    }

    private async Task<ConfigurationDataEntity> LoadConfigurationUnsafe(string configurationKey, string configurationTag,
        CancellationToken cancellationToken)
    {
        IMongoCollection<ConfigurationDataEntity> configCollection = GetConfigurationCollection();

        var filterBuilder = Builders<ConfigurationDataEntity>.Filter;

        var filter = filterBuilder.And(
            filterBuilder.Eq("key", configurationKey),
            filterBuilder.Eq("tag", configurationTag)
        );

        ConfigurationDataEntity entity = await configCollection.Find(filter).FirstAsync(cancellationToken);

        return entity;
    }
    
    public async Task UpdateConfiguration(UpdateConfigurationContainer updateContainer, CancellationToken cancellationToken)
    {
        await Task.Delay(TimeSpan.FromMilliseconds(5), cancellationToken);
        throw new NotImplementedException();
    }
}