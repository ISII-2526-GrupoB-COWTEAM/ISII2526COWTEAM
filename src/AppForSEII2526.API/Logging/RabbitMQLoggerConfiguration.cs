namespace AppForSEII2526.API.Logging;

public class RabbitMQLoggerConfiguration
{
    public string HostName { get; set; } = null!;
    public int Port { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Exchange { get; set; } = null!;
    public string ExchangeType { get; set; } = null!;
    public bool Durable { get; set; }
}