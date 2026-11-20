using Microsoft.Extensions.Logging;

namespace Mqtt.Handlers;

public sealed class TopicShockHandler : TopicReading
{
    private readonly ILogger<TopicShockHandler> _logger;

    public TopicShockHandler(ILogger<TopicShockHandler> logger)
        : base(logger) => _logger = logger;

    public override string Key => "TopicShock";

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
