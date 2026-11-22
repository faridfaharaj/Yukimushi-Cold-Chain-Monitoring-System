using System.Buffers.Binary;
using Microsoft.Extensions.Logging;
using Readings;
using Entities;

namespace Mqtt.Handlers;

public sealed class TopicLightHandler : TopicReading
{
    public TopicLightHandler(ILogger<TopicCoverHandler> logger, IReadingService readingService)
        : base(logger, readingService) { }

    public override string Key => "TopicLight";

    protected override Task HandleReadingAsync(
        string device,
        string kind,
        DateTime timeStamp,
        ReadOnlySpan<byte> rest,
        CancellationToken ct
    )
    {
        var value = BinaryPrimitives.ReadSingleLittleEndian(rest);

        return _readingService.createFloatAsync(
            new FloatReading
            {
                device = device,
                kind = kind,
                timeStamp = timeStamp,
                value = value,
            },
            ct
        );
    }
}
