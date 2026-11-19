namespace Mqtt.Configuration;

public class MqttOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 1883;
    public string ClientId { get; set; } = "cold-chain-monitor";

    public string? Username { get; set; }
    public string? Password { get; set; }

    public bool UseTls { get; set; }

    public Dictionary<string, string> Subscriptions { get; set; } = new();
}
