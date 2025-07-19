namespace Streamline.Messaging.Interfaces;

public interface IMessageHandler<in TMessage>
    where TMessage : notnull, IMessage
{
    Task HandleAsync(TMessage message, CancellationToken token = default);
}