namespace Streamline.Messaging.Builders;

public sealed class ConsumerBuilder
{
    private readonly MessagingOptions _options;
    private readonly Type _messageType;
    private readonly Type _handlerType;

    internal ConsumerBuilder(MessagingOptions options, Type messageType, Type handlerType)
    {
        _options = options;
        _messageType = messageType;
        _handlerType = handlerType;
    }

    public MessagingOptions FromTopic(string subject)
    {
        _options.RegisterConsumer(_messageType, _handlerType, subject, null);

        return _options;
    }

    public MessagingOptions FromQueue(string queueGroup)
    {
        _options.RegisterConsumer(_messageType, _handlerType, _messageType.Name, queueGroup);

        return _options;
    }
}
