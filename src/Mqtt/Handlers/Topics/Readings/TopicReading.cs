using System.Buffers.Binary;
using Microsoft.Extensions.Logging;
using Mqtt.Contracts;

namespace Mqtt.Handlers;

public abstract class TopicReading : ITopicHandler
{
    private readonly ILogger _logger;

    protected TopicReading(ILogger logger) => _logger = logger;

    public abstract string Key { get; }

    protected abstract Task HandleReadingAsync(
        string device,
        DateTime timestamp,
        ReadOnlySpan<byte> bytes,
        CancellationToken ct
    );

    public Task HandleAsync(MqttMessage message, CancellationToken ct)
    {
        var parts = message.Topic.Split('/');
        if (parts.Length < 2 || message.Payload.Length < sizeof(ulong))
        {
            _logger.LogWarning("got bad message from {Topic}", message.Topic);
            return Task.CompletedTask;
        }

        var device = parts[^2];
        var raw = BinaryPrimitives.ReadUInt64LittleEndian(message.Payload);
        var timestamp = DateTimeOffset.FromUnixTimeSeconds((long)raw).UtcDateTime;
        var rest = message.Payload.AsSpan(sizeof(ulong));

        return HandleReadingAsync(device, timestamp, rest, ct);
    }
}
