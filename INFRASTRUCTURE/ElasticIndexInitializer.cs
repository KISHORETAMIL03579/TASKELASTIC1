using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;

namespace INFRASTRUCTURE.Elasticsearch
{
    public class ElasticIndexInitializer
    {
        private readonly ElasticsearchClient _client;
        private readonly ILogger<ElasticIndexInitializer> _logger;

        public ElasticIndexInitializer(ElasticsearchClient client, ILogger<ElasticIndexInitializer> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task CreateIndexIfNotExists()
        {
            const string indexName = "products";

            try
            {
                var exists = await _client.Indices.ExistsAsync(indexName);

                if (exists.Exists)
                {
                    _logger.LogInformation("Index '{Index}' already exists.", indexName);
                    return;
                }

                var response = await _client.Indices.CreateAsync(indexName);

                if (response is not null && response.IsValidResponse)
                {
                    _logger.LogInformation("Index '{Index}' created successfully.", indexName);
                }
                else
                {
                    _logger.LogError("Failed to create index '{Index}': {DebugInfo}", indexName, response?.DebugInformation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while initializing Elasticsearch index '{Index}'", indexName);
                throw;
            }
        }
    }
}
