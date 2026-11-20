using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Mqtt.Configuration;
using Mqtt.Handlers;
using Mqtt.Interfaces;
using Mqtt.Services;

namespace Mqtt;

public static class DependencyInjection
{
    public static IServiceCollection AddMqtt(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MqttOptions>(configuration.GetSection("Mqtt"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MqttOptions>>().Value);

        services.TryAddSingleton<IMqttMessageHandler, MessageDispatcher>();
        services.TryAddSingleton<ITopicHandler, TopicTemperatureHandler>();

        services.AddSingleton<MqttClientService>();
        services.AddSingleton<IMqttClientService>(sp => sp.GetRequiredService<MqttClientService>());

        services.AddHostedService(sp => sp.GetRequiredService<MqttClientService>());

        return services;
    }
}
