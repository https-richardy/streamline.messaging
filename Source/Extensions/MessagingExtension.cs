using Microsoft.Extensions.DependencyInjection;
using Streamline.Messaging.Abstractions;
using Streamline.Messaging.Implementations;

namespace Streamline.Messaging.Extensions;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, Action<MessagingOptions> configure)
    {
        var options = new MessagingOptions();

        configure(options);

        services.AddSingleton(options);
        services.AddSingleton<IMessageProducer, NatsMessageProducer>();

        if (options.Consumers.Count != 0)
        {
            services.AddHostedService<NatsMessageConsumer>();

            foreach (var consumer in options.Consumers)
            {
                services.AddTransient(consumer.HandlerType);
            }

        }

        return services;
    }
}
