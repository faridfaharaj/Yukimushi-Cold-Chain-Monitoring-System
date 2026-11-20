using Mqtt.Contracts;

public interface ITopicHandler
{
    string Key { get; }
    Task HandleAsync(MqttMessage message, CancellationToken ct);
}
