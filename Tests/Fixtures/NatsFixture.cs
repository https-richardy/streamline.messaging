using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace Streamline.Messaging.TestSuite.Fixtures;

public sealed class NatsFixture : IAsyncLifetime
{
    private IContainer _container = default!;
    public string ConnectionString { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        _container = new ContainerBuilder()
            .WithImage("nats:latest")
            .WithCleanUp(true)
            .WithExposedPort(4222)
            .WithPortBinding(0, 4222)
            .Build();

        await _container.StartAsync();
        var hostPort = _container.GetMappedPublicPort(4222);

        ConnectionString = $"nats://localhost:{hostPort}";
    }

    public async Task DisposeAsync()
    {
        await _container.StopAsync();
    }
}
