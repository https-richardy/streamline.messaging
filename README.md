# Streamline.Messaging

A lightweight and elegant abstraction over [NATS](https://nats.io/) for .NET — ideal for fast integration between services without boilerplate code or bloated configuration.

Forget about verbose setups. Just tell the system **what message you're handling or producing**, and Streamline.Messaging takes care of the rest.

> **Not Kafka. Not RabbitMQ.**  
> Just a clean, minimal messaging layer for developers who care about writing readable and maintainable code.

---

## 📦 Features

- Simple & intuitive usage
- Easy message handler registration
- Publish/Subscribe with queues or topics
- Fast to integrate and great for learning messaging in microservices

---

## ✨ Getting Started

### 1. Install via NuGet

```bash
dotnet add package Streamline.Messaging
```

### 2. Define a Message

```csharp
public sealed record OrderCreatedEvent(string OrderId) : IMessage;
```

### 3. Create a Message Handler

```csharp
public sealed class OrderCreatedHandler(IOrderService service): 
    IMessageHandler<OrderCreatedEvent>
{
    public async Task HandleAsync(OrderCreatedEvent message, CancellationToken token = default)
    {
        Console.WriteLine($"Handling order: {message.OrderId}");
    }
}
```

### 4. Configure Consumers and Producers
Inside your Program.cs

```csharp
builder.Services.AddMessaging(options =>
{
    options.AddConsumer<OrderCreatedEvent, OrderCreatedHandler>()
           .FromQueue("orders.created"); // You can also use .FromTopic("some.topic") if preferred.

    options.AddProducer<OrderCreatedEvent>()
           .ToTopic("orders.created");
});
````

### 5. Publish a Message

```csharp
public sealed class SampleHandler(IMessageProducer messageProducer) :
    IRequestHandler<OrderCreationRequest, Result>
{
    public async Task<Result> HandleAsync(OrderCreationRequest request, CancellationToken token)
    {
        var @event = new OrderCreatedEvent
        {
            OrderId = "order_XXXXXXXXX"
        };

        // You can also pass the cancellation token if needed:
        // await messageProducer.PublishAsync(@event, token);

        await messageProducer.PublishAsync(@event);

        return Result.Success();
    }
}
```

---

## ⚠️ Note About Abstractions

If you prefer to **keep your domain and application layers decoupled from infrastructure**, or are following **Clean Architecture** principles, consider using the separate package:

### [Streamline.Messaging.Abstractions](https://github.com/https-richardy/streamline.messaging.abstractions)

This package provides **only the messaging interfaces and contracts** — no implementation. Perfect for defining messaging contracts in shared libraries, writing unit tests, and keeping your domain clean.

### Installation

You can install the abstractions package via NuGet:

```bash
dotnet add package Streamline.Messaging.Abstractions
```
---