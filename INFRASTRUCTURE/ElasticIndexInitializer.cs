using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace INFRASTRUCTURE.Elasticsearch
{
    public class ElasticIndexInitializer
    {
        private readonly ElasticsearchClient _client;
        private readonly ILogger<ElasticIndexInitializer> _logger;

        public ElasticIndexInitializer(
            ElasticsearchClient client,
            ILogger<ElasticIndexInitializer> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task CreateIndexIfNotExists()
        {
            const string indexName = "products";

            var exists = await _client.Indices.ExistsAsync(indexName);

            if (exists.Exists)
            {
                _logger.LogInformation(
                    "Index '{Index}' already exists.",
                    indexName);

                return;
            }

            var response = await _client.Indices.CreateAsync(indexName, c => c
            .Settings(s => s
            .NumberOfShards(1)
            .NumberOfReplicas(0)
                .Analysis(a => a
                    .Analyzers(an => an
                        .Custom("product_analyzer", ca => ca
                            .Tokenizer("standard")
                                .Filter(new[] { "lowercase" , "edge_ngram_filter" })
                            )
                        )
                        .TokenFilters(tf => tf
                            .EdgeNGram("edge_ngram_filter", e => e
                                .MinGram(2)
                                    .MaxGram(10)
                            )
                        )
                    )
                )
                .Mappings(m => m
                    .Properties(new Properties
                    {
                        { "id", new KeywordProperty() },
                        {"name", new TextProperty
                        {
                            Analyzer = "product_analyzer",
                            Fields = new Properties
                            {
                                { "keyword", new KeywordProperty() }
                            }
                        }},
                        { "price", new DoubleNumberProperty() },
                        { "stock", new IntegerNumberProperty() }
                    })
                ));

            if (response.IsValidResponse)
            {
                _logger.LogInformation(
                    "Index '{Index}' created successfully.",
                    indexName);
            }
            if (!response.IsValidResponse)
            {
                _logger.LogError("Failed to create index '{Index}'", indexName);
            }
        }
    }
}