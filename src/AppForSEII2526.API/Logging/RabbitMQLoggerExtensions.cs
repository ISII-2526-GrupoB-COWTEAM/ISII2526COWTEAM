using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AppForSEII2526.API.Logging;

public static class RabbitMQLoggerExtensions
{
    public static ILoggingBuilder AddRabbitMQ(
        this ILoggingBuilder builder,
        IConfigurationSection config)
    {
        builder.Services.Configure<RabbitMQLoggerConfiguration>(config);
        builder.Services.AddSingleton<ILoggerProvider, RabbitMQLoggerProvider>();
        return builder;
    }
}