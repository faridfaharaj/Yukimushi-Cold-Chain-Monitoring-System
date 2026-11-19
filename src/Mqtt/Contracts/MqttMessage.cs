namespace Mqtt.Contracts;

public sealed record MqttMessage(
    string Topic,
    byte[] Payload,
    int QualityOfService,
    bool Retain,
    DateTimeOffset ReceivedAt
);
