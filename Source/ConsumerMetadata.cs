namespace Streamline.Messaging;

internal sealed class ConsumerMetadata
{
    public Type MessageType { get; private set; } = default!;
    public Type HandlerType { get; private set; } = default!;

    public string Subject { get; private set; } = default!;
    public string? QueueGroup { get; private set; }

    public ConsumerMetadata SetMessageType(Type type)
    {
        MessageType = type;
        return this;
    }

    public ConsumerMetadata SetHandlerType(Type type)
    {
        HandlerType = type;
        return this;
    }

    public ConsumerMetadata SetSubject(string subject)
    {
        Subject = subject;
        return this;
    }

    public ConsumerMetadata SetQueueGroup(string? queueGroup)
    {
        QueueGroup = queueGroup;
        return this;
    }
}
