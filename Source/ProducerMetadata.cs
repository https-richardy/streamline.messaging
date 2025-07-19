namespace Streamline.Messaging;

internal sealed class ProducerMetadata
{
    public Type MessageType { get; private set; } = default!;

    public string Subject { get; private set; } = default!;
    public string? QueueGroup { get; private set; }

    public ProducerMetadata SetMessageType(Type type)
    {
        MessageType = type;
        return this;
    }

    public ProducerMetadata SetSubject(string subject)
    {
        Subject = subject;
        return this;
    }

    public ProducerMetadata SetQueueGroup(string? queueGroup)
    {
        QueueGroup = queueGroup;
        return this;
    }
}
