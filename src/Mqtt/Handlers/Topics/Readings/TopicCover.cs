using Entities;
using Microsoft.Extensions.Logging;
using Readings;

namespace Mqtt.Handlers;

public sealed class TopicCoverHandler : TopicReading
{
    public TopicCoverHandler(ILogger<TopicCoverHandler> logger, IReadingService readingService)
        : base(logger, readingService) { }

    public override string Key => "TopicCover";

    protected override Task HandleReadingAsync(
        string device,
        string kind,
        DateTime timeStamp,
        ReadOnlySpan<byte> rest,
        CancellationToken ct
    )
    {
        var state = rest.Length > 0 && rest[0] != 0;

        return _readingService.createBooleanAsync(
            new BooleanReading
            {
                device = device,
                kind = kind,
                timeStamp = timeStamp,
                state = state,
            },
            ct
        );
    }
}
