using Microsoft.Extensions.Logging;

namespace Mqtt.Handlers;

public sealed class TopicTippedHandler : TopicReading
{
    private readonly ILogger<TopicTippedHandler> _logger;

    public TopicTippedHandler(ILogger<TopicTippedHandler> logger)
        : base(logger) => _logger = logger;

    public override string Key => "TopicTipped";

    protected override Task HandleReadingAsync(
        string device,
        DateTime timeStamp,
        ReadOnlySpan<byte> rest,
        CancellationToken ct
    )
    {
        //handle
        return Task.CompletedTask;
    }
}
