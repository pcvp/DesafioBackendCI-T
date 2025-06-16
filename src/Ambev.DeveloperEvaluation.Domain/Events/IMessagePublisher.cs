namespace Ambev.DeveloperEvaluation.Domain.Events;

/// <summary>
/// Interface for publishing messages to a message broker
/// </summary>
public interface IMessagePublisher
{
    /// <summary>
    /// Publishes a message to the specified topic
    /// </summary>
    /// <param name="topic">The topic/queue name</param>
    /// <param name="message">The message to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task PublishAsync(string topic, object message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes a typed message to the specified topic
    /// </summary>
    /// <typeparam name="T">The message type</typeparam>
    /// <param name="topic">The topic/queue name</param>
    /// <param name="message">The message to publish</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task PublishAsync<T>(string topic, T message, CancellationToken cancellationToken = default) where T : class;
} 