using System.Buffers.Binary;
using Microsoft.Extensions.Logging;

namespace Mqtt.Handlers;

public sealed class TopicCoverHandler : TopicReading
{
    private readonly ILogger<TopicCoverHandler> _logger;

    public TopicCoverHandler(ILogger<TopicCoverHandler> logger)
        : base(logger) => _logger = logger;

    public override string Key => "TopicCover";

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
