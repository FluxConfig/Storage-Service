using FluxConfig.Storage.Domain.Contracts.Dal.Entities;

namespace FluxConfig.Storage.Domain.Contracts.Dal.Interfaces;

public interface IRealTimeConfigurationRepository
{
    public Task<ConfigurationDataEntity> LoadConfiguration(string serviceApiKey, string configurationTag,
        CancellationToken cancellationToken);
}