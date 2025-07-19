using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Streamline.Messaging.Implementations;

internal sealed class NatsMessageConsumer : IHostedService, IDisposable
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private readonly MessagingOptions _options;
    private readonly List<IAsyncSubscription> _subscriptions = [  ];

    public NatsMessageConsumer(IServiceProvider serviceProvider, MessagingOptions options)
    {
        var factory = new ConnectionFactory();

        _serviceProvider = serviceProvider;
        _options = options;
        _connection = factory.CreateConnection(options.ConnectionString);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var consumer in _options.Consumers)
        {
            if (consumer.QueueGroup is not null)
            {
                var subscription = _connection.SubscribeAsync(consumer.Subject, consumer.QueueGroup, (sender, args) => HandleMessageAsync(consumer, args.Message));
                _subscriptions.Add(subscription);
            }
            else
            {
                var subscription = _connection.SubscribeAsync(consumer.Subject, (sender, args) => HandleMessageAsync(consumer, args.Message));
                _subscriptions.Add(subscription);
            }
        }

        return Task.CompletedTask;
    }

    private async void HandleMessageAsync(ConsumerMetadata metadata, Msg natsMessage)
    {
        using var scope = _serviceProvider.CreateScope();

        var handler = scope.ServiceProvider.GetRequiredService(metadata.HandlerType);
        var message = JsonSerializer.Deserialize(natsMessage.Data, metadata.MessageType);

        var handleAsyncMethod = metadata.HandlerType.GetMethod("HandleAsync");
        if (handleAsyncMethod is not null && message is not null)
        {
            var task = (Task?) handleAsyncMethod.Invoke(handler, [message, CancellationToken.None]);
            if (task is not null)
            {
                await task.ConfigureAwait(false);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Unsubscribe();
        }

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Dispose();
        }

        _connection?.Dispose();
    }
}
