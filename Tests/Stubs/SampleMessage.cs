namespace Streamline.Messaging.TestSuite.Stubs;

public sealed record SampleMessage : IMessage
{
    public string Content { get; set; } = default!;
    public string Description { get; set; } = default!;
}