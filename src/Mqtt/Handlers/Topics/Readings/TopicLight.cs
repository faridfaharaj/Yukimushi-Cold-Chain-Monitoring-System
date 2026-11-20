using System.Buffers.Binary;
using Microsoft.Extensions.Logging;

namespace Mqtt.Handlers;

public sealed class TopicLightHandler : TopicReading
{
    private readonly ILogger<TopicLightHandler> _logger;

    public TopicLightHandler(ILogger<TopicLightHandler> logger)
        : base(logger) => _logger = logger;

    public override string Key => "TopicLight";

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
