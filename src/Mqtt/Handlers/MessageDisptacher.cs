using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mqtt.Configuration;
using Mqtt.Contracts;
using Mqtt.Interfaces;
using MQTTnet;

namespace Mqtt.Handlers;

public sealed class MessageDispatcher : IMqttMessageHandler
{
    private readonly (string Filter, string Key)[] _routes;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MessageDispatcher> _logger;

    public MessageDispatcher(
        MqttOptions options,
        IServiceScopeFactory scopeFactory,
        ILogger<MessageDispatcher> logger
    )
    {
        _logger = logger;
        _scopeFactory = scopeFactory;

        _routes = options.Subscriptions.Select(s => (s.Value, s.Key)).ToArray();
    }

    public async Task HandleAsync(MqttMessage message, CancellationToken ct)
    {
        var matched = false;

        foreach (var (filter, key) in _routes)
        {
            if (
                MqttTopicFilterComparer.Compare(message.Topic, filter)
                != MqttTopicFilterCompareResult.IsMatch
            )
                continue;

            await using var scope = _scopeFactory.CreateAsyncScope();

            var handler = scope
                .ServiceProvider.GetServices<ITopicHandler>()
                .FirstOrDefault(h => h.Key == key);

            if (handler is null)
            {
                _logger.LogWarning(
                    "No handler registered for key {Key} (topic {Topic})",
                    key,
                    message.Topic
                );
                continue;
            }

            matched = true;
            await handler.HandleAsync(message, ct);
        }

        if (!matched)
            _logger.LogWarning("No handler for topic {Topic}", message.Topic);
    }
}
