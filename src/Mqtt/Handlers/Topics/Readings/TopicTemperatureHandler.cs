using System.Buffers.Binary;
using Microsoft.Extensions.Logging;

namespace Mqtt.Handlers;

public sealed class TopicTemperatureHandler : TopicReading
{
    private readonly ILogger<TopicTemperatureHandler> _logger;

    public TopicTemperatureHandler(ILogger<TopicTemperatureHandler> logger)
        : base(logger) => _logger = logger;

    public override string Key => "TopicTemperature";

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
