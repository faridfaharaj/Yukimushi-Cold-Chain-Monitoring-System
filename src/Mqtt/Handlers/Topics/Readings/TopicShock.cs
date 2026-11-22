using Entities;
using Microsoft.Extensions.Logging;
using Readings;

namespace Mqtt.Handlers;

public sealed class TopicShockHandler : TopicReading
{
    public TopicShockHandler(ILogger<TopicCoverHandler> logger, IReadingService readingService)
        : base(logger, readingService) { }

    public override string Key => "TopicShock";

    protected override Task HandleReadingAsync(
        string device,
        string kind,
        DateTime timeStamp,
        ReadOnlySpan<byte> rest,
        CancellationToken ct
    )
    {
        return _readingService.createTriggerAsync(
            new TriggerReading
            {
                device = device,
                kind = kind,
                timeStamp = timeStamp,
            },
            ct
        );
    }
}
