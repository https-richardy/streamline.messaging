using Streamline.Messaging.Abstractions;

namespace Streamline.Messaging.TestSuite;

public class MessagingIntegrationTests(NatsFixture fixture) :
    IClassFixture<NatsFixture>
{
    private readonly NatsFixture _fixture = fixture;

    [Fact(DisplayName = "given message is produced when consumed then handler processes and changes content")]
    public async Task Given_MessageIsProduced_When_Consumed_Then_HandlerProcessesAndChangesContent()
    {
        var channel = Channel.CreateUnbounded<SampleMessage>();
        var services = new ServiceCollection();
        var sampleService = new SampleMessageService();

        services.AddSingleton(sampleService);
        services.AddMessaging(options =>
        {
            options.UseConnectionString(_fixture.ConnectionString);

            options.AddConsumer<SampleMessage, SampleMessageHandler>()
                .FromTopic("sample.message");

            options.AddProducer<SampleMessage>()
                .ToTopic("sample.message");
        });

        using var provider = services.BuildServiceProvider();
        var hostedServices = provider.GetServices<IHostedService>();

        foreach (var hostedService in hostedServices)
        {
            await hostedService.StartAsync(default);
        }

        var producer = provider.GetRequiredService<IMessageProducer>();
        var message = new SampleMessage
        {
            Content = "original",
            Description = "original description"
        };

        await producer.PublishAsync(message);
        await Task.Delay(500);

        Assert.Equal("changed", sampleService.ChangeContent(message).Content);

        foreach (var hostedService in hostedServices)
        {
            await hostedService.StopAsync(default);
        }
    }

    [Fact(DisplayName = "given message is produced to a queue when consumed then handler processes and changes content")]
    public async Task Given_MessageIsProducedToQueue_When_Consumed_Then_HandlerProcessesAndChangesContent()
    {
        var services = new ServiceCollection();
        var sampleService = new SampleMessageService();

        services.AddSingleton(sampleService);
        services.AddMessaging(options =>
        {
            options.UseConnectionString(_fixture.ConnectionString);

            options.AddConsumer<SampleMessage, SampleMessageHandler>()
                .FromQueue("sample.message.queue");

            options.AddProducer<SampleMessage>()
                .ToTopic("sample.message.queue");
        });

        using var provider = services.BuildServiceProvider();
        var hostedServices = provider.GetServices<IHostedService>();

        foreach (var hostedService in hostedServices)
        {
            await hostedService.StartAsync(default);
        }

        var producer = provider.GetRequiredService<IMessageProducer>();
        var message = new SampleMessage
        {
            Content = "original",
            Description = "original description"
        };

        await producer.PublishAsync(message);
        await Task.Delay(500);

        Assert.Equal("changed", sampleService.ChangeContent(message).Content);

        foreach (var hostedService in hostedServices)
        {
            await hostedService.StopAsync(default);
        }
    }
}
