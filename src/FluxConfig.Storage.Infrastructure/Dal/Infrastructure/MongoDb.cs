using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Infrastructure;

public static class MongoDb
{
    public static void AddClient(IServiceCollection services, MongoDbConnectionOptions mongoOptions)
    {
        services.AddSingleton<IMongoClient>(new MongoClient(mongoOptions.GenerateMongoClientSettings()));
    }
    
    private static MongoClientSettings GenerateMongoClientSettings(this MongoDbConnectionOptions connectionOptions)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionOptions.ConnectionString);
        settings.Credential = MongoCredential.CreateCredential(
            databaseName: connectionOptions.AuthDb,
            username: connectionOptions.DbUsername,
            password: connectionOptions.DbPassword
        );

        return settings;
    }
}