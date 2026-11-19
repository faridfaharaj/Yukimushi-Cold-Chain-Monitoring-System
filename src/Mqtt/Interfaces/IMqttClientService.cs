namespace Mqtt.Interfaces;

public interface IMqttClientService
{
    bool IsConnected { get; }

    Task ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    Task SubscribeAsync(
        string topic,
        int qualityOfService = 0,
        CancellationToken cancellationToken = default
    );
    Task UnsubscribeAsync(string topic, CancellationToken cancellationToken = default);

    Task PublishAsync(
        string topic,
        byte[] payload,
        int qualityOfService = 0,
        bool retain = false,
        CancellationToken cancellationToken = default
    );
}
