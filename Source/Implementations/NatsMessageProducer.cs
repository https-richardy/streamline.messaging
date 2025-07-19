namespace Streamline.Messaging.Implementations;

public class NatsMessageProducer : IMessageProducer, IDisposable
{
    private readonly IConnection _connection;
    private readonly Dictionary<Type, string> _subjectMap;

    public NatsMessageProducer(MessagingOptions options)
    {
        var connectionFactory = new ConnectionFactory();

        _connection = connectionFactory.CreateConnection(options.ConnectionString);
        _subjectMap = options.Producers.GroupBy(producer => producer.MessageType)
            .ToDictionary(group => group.Key, group => group.First().Subject);
    }

    public Task PublishAsync<TMessage>(TMessage message, CancellationToken token = default)
        where TMessage :
        notnull, IMessage
    {
        var subject = _subjectMap.TryGetValue(typeof(TMessage), out var subjectObject) ?
            subjectObject : throw new InvalidOperationException(
            $"Subject not configured for message type {typeof(TMessage).Name}"
        );

        var payload = JsonSerializer.SerializeToUtf8Bytes(message);

        _connection.Publish(subject, payload);
        _connection.Flush();

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
