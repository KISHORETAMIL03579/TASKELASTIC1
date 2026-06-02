using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using INFRASTRUCTURE.Elasticsearch;
using INFRASTRUCTURE.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read config
var esSettings = builder.Configuration.GetSection("Elasticsearch").Get<ElasticsearchSettings>();

if (esSettings is null)
{
    throw new Exception("Elasticsearch configuration is missing");
}

// Elasticsearch client
var clientSettings = new ElasticsearchClientSettings(new Uri(esSettings.Url))
    .Authentication(new BasicAuthentication(esSettings.Username, esSettings.Password));

var client = new ElasticsearchClient(clientSettings);

// Register DI
builder.Services.AddSingleton(client);
builder.Services.AddSingleton<ElasticIndexInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<ElasticIndexInitializer>();
    await initializer.CreateIndexIfNotExists();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();