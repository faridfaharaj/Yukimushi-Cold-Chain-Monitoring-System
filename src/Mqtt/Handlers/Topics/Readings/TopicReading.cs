using System.Buffers.Binary;
using Microsoft.Extensions.Logging;
using Mqtt.Contracts;
using Readings;

namespace Mqtt.Handlers;

public abstract class TopicReading : ITopicHandler
{
    protected readonly ILogger _logger;
    protected readonly IReadingService _readingService;

    protected TopicReading(ILogger logger, IReadingService readingService)
    {
        _logger = logger;
        _readingService = readingService;
    }

    public abstract string Key { get; }

    protected abstract Task HandleReadingAsync(
        string device,
        string kind,
        DateTime timestamp,
        ReadOnlySpan<byte> bytes,
        CancellationToken ct
    );

    public Task HandleAsync(MqttMessage message, CancellationToken ct)
    {
        var parts = message.Topic.Split('/');
        if (parts.Length < 2 || message.Payload.Length < sizeof(uint))
        {
            _logger.LogWarning("got bad message from {Topic}", message.Topic);
            return Task.CompletedTask;
        }

        var device = parts[^2];
        var raw = BinaryPrimitives.ReadUInt32LittleEndian(message.Payload);
        var timestamp = DateTimeOffset.FromUnixTimeSeconds(raw).UtcDateTime;
        var rest = message.Payload.AsSpan(sizeof(uint));

        return HandleReadingAsync(device, Key, timestamp, rest, ct);
    }
}
