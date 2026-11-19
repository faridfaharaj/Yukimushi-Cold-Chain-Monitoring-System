using Microsoft.Extensions.Logging;
using Mqtt.Contracts;
using Mqtt.Interfaces;

namespace Mqtt.Handlers;

public class SensorMessageHandler(ILogger<SensorMessageHandler> logger) : IMqttMessageHandler
{
    public Task HandleAsync(MqttMessage message, CancellationToken cancellationToken)
    {
        var splitTopic = message.Topic.Split("/");

        if (splitTopic.Length >= 3 && splitTopic[^3] == "readings")
        {
            switch (splitTopic[splitTopic.Length - 1])
            {
                case "temperature":
                    // handle
                    break;
                case "humidity":
                    // handle
                    break;
                case "light":
                    // handle
                    break;
                case "shock":
                    // handle
                    break;
                case "cover":
                    // handle
                    break;
                case "tipped":
                    // handle
                    break;
            }
        }

        logger.LogInformation("Received {Topic}", message.Topic);

        return Task.CompletedTask;
    }
}
