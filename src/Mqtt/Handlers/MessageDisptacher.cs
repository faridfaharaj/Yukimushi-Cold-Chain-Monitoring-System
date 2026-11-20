using Microsoft.Extensions.Logging;
using Mqtt.Configuration;
using Mqtt.Contracts;
using Mqtt.Interfaces;
using MQTTnet;
namespace Mqtt.Handlers;

public sealed class MessageDispatcher : IMqttMessageHandler
{
    private readonly (string Filter, ITopicHandler Handler)[] _routes;
    private readonly ILogger<MessageDispatcher> _logger;

    public MessageDispatcher(
        MqttOptions options,
        IEnumerable<ITopicHandler> handlers,
        ILogger<MessageDispatcher> logger
    )
    {
        _logger = logger;
        var byKey = handlers.ToDictionary(h => h.Key);

        _routes = options
            .Subscriptions.Where(s => byKey.ContainsKey(s.Key))
            .Select(s => (s.Value, byKey[s.Key]))
            .ToArray();
    }

    public async Task HandleAsync(MqttMessage message, CancellationToken ct)
    {
        var matched = false;
        foreach (var (filter, handler) in _routes)
        {
            if (
                MqttTopicFilterComparer.Compare(message.Topic, filter)
                == MqttTopicFilterCompareResult.IsMatch
            )
            {
                matched = true;
                await handler.HandleAsync(message, ct);
            }
        }

        if (!matched)
            _logger.LogWarning("No handler for topic {Topic}", message.Topic);
    }
}
