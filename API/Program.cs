using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using INFRASTRUCTURE.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ✅ Read configuration safely
var esSettings = builder.Configuration
    .GetSection("Elasticsearch")
    .Get<ElasticsearchSettings>();

if (esSettings is null)
{
    throw new Exception("Elasticsearch configuration is missing");
}


// ✅ Create Elasticsearch client
var clientSettings = new ElasticsearchClientSettings(new Uri(esSettings.Url))
    .Authentication(new BasicAuthentication(esSettings.Username, esSettings.Password));

var client = new ElasticsearchClient(clientSettings);


// ✅ Register DI
builder.Services.AddSingleton(client);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();