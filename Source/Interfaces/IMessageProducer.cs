namespace Streamline.Messaging.Interfaces;

public interface IMessageProducer
{
    Task PublishAsync<TMessage>(TMessage message, CancellationToken token = default)
        where TMessage :
        notnull, IMessage;
}