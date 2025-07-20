namespace Streamline.Messaging.Builders;

public sealed class ProducerBuilder
{
    private readonly MessagingOptions _options;
    private readonly Type _messageType;

    private string? _subject;
    private string? _queueGroup;

    internal ProducerBuilder(MessagingOptions options, Type messageType)
    {
        _options = options;
        _messageType = messageType;
    }

    public MessagingOptions ToTopic(string subject)
    {
        _subject = subject;

        Register();

        return _options;
    }

    public MessagingOptions ToQueue(string queueGroup)
    {
        _queueGroup = queueGroup;
        _subject = _messageType.Name;

        Register();

        return _options;
    }

    private void Register()
    {
        _options.RegisterProducer(_messageType, _subject!, _queueGroup);
    }
}
