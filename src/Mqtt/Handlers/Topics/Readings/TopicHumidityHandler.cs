using System.Buffers.Binary;
using Microsoft.Extensions.Logging;

namespace Mqtt.Handlers;

public sealed class TopicHumidityHandler : TopicReading
{
    private readonly ILogger<TopicHumidityHandler> _logger;

    public TopicHumidityHandler(ILogger<TopicHumidityHandler> logger)
        : base(logger) => _logger = logger;

    public override string Key => "TopicHumidity";

    protected override Task HandleReadingAsync(
        string device,
        DateTime timeStamp,
        ReadOnlySpan<byte> rest,
        CancellationToken ct
    )
    {
        var value = BinaryPrimitives.ReadSingleLittleEndian(rest);
        //handle
        return Task.CompletedTask;
    }
}
