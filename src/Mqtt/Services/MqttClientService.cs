using System.Buffers;
using System.Collections.Concurrent;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mqtt.Configuration;
using Mqtt.Contracts;
using Mqtt.Interfaces;
using MQTTnet;
using MQTTnet.Protocol;

namespace Mqtt.Services;

public sealed class MqttClientService : IMqttClientService, IHostedService, IAsyncDisposable
{
    private readonly IMqttClient _client;
    private readonly MqttClientOptions _options;
    private readonly string[] _topics;
    private readonly IMqttMessageHandler _handler;
    private readonly ILogger<MqttClientService> _logger;
    private readonly ConcurrentDictionary<string, int> _subscriptions = new();
    private readonly CancellationTokenSource _shutdown = new();
    private bool _manualDisconnect;

    public MqttClientService(
        IOptions<MqttOptions> options,
        IMqttMessageHandler handler,
        ILogger<MqttClientService> logger
    )
    {
        var o = options.Value;
        _topics = o.Subscriptions.Values.Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();

        var builder = new MqttClientOptionsBuilder()
            .WithTcpServer(o.Host, o.Port)
            .WithClientId(o.ClientId);

        if (o.UseTls)
            builder = builder.WithTlsOptions(t => t.UseTls());

        if (!string.IsNullOrWhiteSpace(o.Username))
            builder = builder.WithCredentials(o.Username, o.Password);

        _options = builder.Build();

        _handler = handler;
        _logger = logger;
        _client = new MqttClientFactory().CreateMqttClient();

        _client.ApplicationMessageReceivedAsync += OnMessageReceivedAsync;
        _client.ConnectedAsync += OnConnectedAsync;
        _client.DisconnectedAsync += _ => ReconnectLoopAsync();
    }

    public bool IsConnected => _client.IsConnected;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var topic in _topics)
            _subscriptions[topic] = 1;

        try
        {
            await ConnectAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "Initial MQTT connect failed, retrying in background");
            _ = ReconnectLoopAsync();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) =>
        DisconnectAsync(cancellationToken);

    private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs e)
    {
        var m = e.ApplicationMessage;
        var message = new MqttMessage(
            m.Topic,
            m.Payload.ToArray(),
            (int)m.QualityOfServiceLevel,
            m.Retain,
            DateTimeOffset.UtcNow
        );

        try
        {
            await _handler.HandleAsync(message, _shutdown.Token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Handler failed for topic {Topic}", m.Topic);
        }
    }

    private async Task OnConnectedAsync(MqttClientConnectedEventArgs _)
    {
        foreach (var (topic, qos) in _subscriptions)
        {
            try
            {
                await _client.SubscribeAsync(topic, (MqttQualityOfServiceLevel)qos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Subscribe failed for {Topic}", topic);
            }
        }
    }

    private async Task ReconnectLoopAsync()
    {
        while (!_manualDisconnect && !_shutdown.IsCancellationRequested && !_client.IsConnected)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(5), _shutdown.Token);
                await _client.ConnectAsync(_options, _shutdown.Token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Reconnect failed, retrying");
            }
        }
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        _manualDisconnect = false;
        if (_client.IsConnected)
            return;
        await _client.ConnectAsync(_options, cancellationToken);
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        _manualDisconnect = true;
        if (!_client.IsConnected)
            return;
        await _client.DisconnectAsync(cancellationToken: cancellationToken);
    }

    public async Task SubscribeAsync(
        string topic,
        int qualityOfService = 0,
        CancellationToken cancellationToken = default
    )
    {
        _subscriptions[topic] = qualityOfService;
        if (_client.IsConnected)
            await _client.SubscribeAsync(
                topic,
                (MqttQualityOfServiceLevel)qualityOfService,
                cancellationToken
            );
    }

    public async Task UnsubscribeAsync(string topic, CancellationToken cancellationToken = default)
    {
        _subscriptions.TryRemove(topic, out _);
        if (_client.IsConnected)
            await _client.UnsubscribeAsync(topic, cancellationToken);
    }

    public Task PublishAsync(
        string topic,
        byte[] payload,
        int qualityOfService = 0,
        bool retain = false,
        CancellationToken cancellationToken = default
    )
    {
        var msg = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel((MqttQualityOfServiceLevel)qualityOfService)
            .WithRetainFlag(retain)
            .Build();

        return _client.PublishAsync(msg, cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        _manualDisconnect = true;
        _shutdown.Cancel();
        if (_client.IsConnected)
            await _client.DisconnectAsync();
        _client.Dispose();
        _shutdown.Dispose();
    }
}
