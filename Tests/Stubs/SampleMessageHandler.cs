using Streamline.Messaging.Abstractions;

namespace Streamline.Messaging.TestSuite.Stubs;

public sealed class SampleMessageHandler(SampleMessageService service) :
    IMessageHandler<SampleMessage>
{
    public async Task HandleAsync(SampleMessage message, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>service.ChangeContent(message));
    }
}
