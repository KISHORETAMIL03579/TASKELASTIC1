namespace INFRASTRUCTURE.Configuration;

public class ElasticsearchSettings
{
    public required string Url { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}