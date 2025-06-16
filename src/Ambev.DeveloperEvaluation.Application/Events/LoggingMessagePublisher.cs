using System.Text.Json;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Implementation of IMessagePublisher that logs messages instead of publishing to a real message broker
/// </summary>
public class LoggingMessagePublisher : IMessagePublisher
{
    private readonly ILogger<LoggingMessagePublisher> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of LoggingMessagePublisher
    /// </summary>
    /// <param name="logger">The logger instance</param>
    public LoggingMessagePublisher(ILogger<LoggingMessagePublisher> logger)
    {
        _logger = logger;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    /// <summary>
    /// Publishes a message by logging it
    /// </summary>
    /// <param name="topic">The topic/queue name</param>
    /// <param name="message">The message to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task PublishAsync(string topic, object message, CancellationToken cancellationToken = default)
    {
        try
        {
            var messageJson = JsonSerializer.Serialize(message, _jsonOptions);
            var messageType = message.GetType().Name;

            _logger.LogInformation(
                "📨 MESSAGE PUBLISHED to topic '{Topic}' | Type: {MessageType} | Message: {Message}",
                topic,
                messageType,
                messageJson
            );

            // Simulate async operation
            await Task.Delay(1, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, 
                "❌ Failed to publish message to topic '{Topic}' | MessageType: {MessageType}",
                topic,
                message.GetType().Name
            );
            throw;
        }
    }

    /// <summary>
    /// Publishes a typed message by logging it
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="topic">The topic/queue name</param>
    /// <param name="message">The message to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class
    {
        await PublishAsync(topic, (object)message, cancellationToken);
    }
} 