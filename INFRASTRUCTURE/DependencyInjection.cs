using APPLICATION.AutoMapper;
using APPLICATION.Interfaces;
using APPLICATION.Services;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using INFRASTRUCTURE.Configuration;
using INFRASTRUCTURE.Elasticsearch;
using INFRASTRUCTURE.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind settings using Options pattern
        services.Configure<ElasticsearchSettings>(configuration.GetSection("Elasticsearch"));

        // Register Elasticsearch client. For development, accept untrusted TLS certificates.
        services.AddSingleton(sp =>
        {
            var options = sp.GetRequiredService<IOptions<ElasticsearchSettings>>().Value;

            var clientSettings = new ElasticsearchClientSettings(new Uri(options.Url))
                .Authentication(new BasicAuthentication(options.Username, options.Password))
                .ServerCertificateValidationCallback((sender, cert, chain, errors) => true); // dev-only

            return new ElasticsearchClient(clientSettings);
        });
        services.AddSingleton<ElasticIndexInitializer>();
        services.AddAutoMapper(typeof(AutoMap).Assembly);
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        return services;
    }
}