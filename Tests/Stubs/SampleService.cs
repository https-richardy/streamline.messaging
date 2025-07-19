namespace Streamline.Messaging.TestSuite.Stubs;

public sealed class SampleMessageService
{
    public SampleMessage ChangeContent(SampleMessage message)
    {
        message.Content = "changed";
        message.Description = "changed";

        return message;
    }
}