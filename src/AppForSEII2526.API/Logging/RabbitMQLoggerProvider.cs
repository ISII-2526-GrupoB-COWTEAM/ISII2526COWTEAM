using AppForSEII2526.API.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppForSEII2526.API.Logging;

[ProviderAlias("RabbitMQ")]
public class RabbitMQLoggerProvider : ILoggerProvider
{
    private readonly RabbitMQLoggerConfiguration _config;
    private readonly Dictionary<string, RabbitMQLogger> _loggers = new();
    private readonly Object _lock = new Object();

    public RabbitMQLoggerProvider(IOptions<RabbitMQLoggerConfiguration> config)
    {
        _config = config.Value;
    }

    public ILogger CreateLogger(string categoryName)
    {
        lock (_lock)
        {
            if (!_loggers.TryGetValue(categoryName, out var logger))
            {
                logger = new RabbitMQLogger(categoryName, _config);
                _loggers[categoryName] = logger;
            }

            return logger;
        }
    }

    public void Dispose()
    {
        foreach (var logger in _loggers.Values)
        {
            logger.Dispose();
        }

        _loggers.Clear();
    }
}