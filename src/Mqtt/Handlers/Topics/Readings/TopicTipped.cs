using Entities;
using Microsoft.Extensions.Logging;
using Readings;

namespace Mqtt.Handlers;

public sealed class TopicTippedHandler : TopicReading
{
    public TopicTippedHandler(ILogger<TopicCoverHandler> logger, IReadingService readingService)
        : base(logger, readingService) { }

    public override string Key => "TopicTipped";

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
