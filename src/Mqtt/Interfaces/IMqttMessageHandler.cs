using Mqtt.Contracts;

namespace Mqtt.Interfaces;

public interface IMqttMessageHandler
{
    Task HandleAsync(
        MqttMessage message,
        CancellationToken cancellationToken);
}
