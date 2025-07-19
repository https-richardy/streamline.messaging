using Streamline.Messaging.Builders;

namespace Streamline.Messaging;

public sealed class MessagingOptions
{
    internal List<ConsumerMetadata> Consumers { get; private set; } = [  ];
    internal List<ProducerMetadata> Producers { get; private set; } = [  ];

    public string ConnectionString { get; private set; } = default!;

    public MessagingOptions UseConnectionString(string connectionString)
    {
        ConnectionString = connectionString;
        return this;
    }

    public ConsumerBuilder AddConsumer<TMessage, THandler>()
        where TMessage : class, IMessage
        where THandler : class, IMessageHandler<TMessage>
    {
        return new ConsumerBuilder(this, typeof(TMessage), typeof(THandler));
    }

    public ProducerBuilder AddProducer<TMessage>()
        where TMessage : class, IMessage
    {
        return new ProducerBuilder(this, typeof(TMessage));
    }

    internal void RegisterConsumer(Type messageType, Type handlerType, string subject, string? queueGroup)
    {
        var metadata = new ConsumerMetadata();

        metadata.SetHandlerType(handlerType);
        metadata.SetMessageType(messageType);
        metadata.SetSubject(subject);
        metadata.SetQueueGroup(queueGroup);

        Consumers.Add(metadata);
    }

    internal void RegisterProducer(Type messageType, string subject, string? queueGroup)
    {
        var metadata = new ProducerMetadata();

        metadata.SetMessageType(messageType);
        metadata.SetSubject(subject);
        metadata.SetQueueGroup(queueGroup);

        Producers.Add(metadata);
    }
}
