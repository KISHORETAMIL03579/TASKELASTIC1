using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using INFRASTRUCTURE.Elasticsearch;
using INFRASTRUCTURE.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var esSettings = configuration
            .GetSection("Elasticsearch")
            .Get<ElasticsearchSettings>();

        if (esSettings is null)
        {
            throw new Exception("Elasticsearch configuration is missing in appsettings.json");
        }

        var clientSettings = new ElasticsearchClientSettings(new Uri(esSettings.Url))
            .Authentication(new BasicAuthentication(esSettings.Username, esSettings.Password));

        var client = new ElasticsearchClient(clientSettings);

        services.AddSingleton(client);
        services.AddSingleton<ElasticIndexInitializer>();

        return services;
    }
}