using System;
using System.Threading.Tasks;
using LightBDD.Framework.Messaging;
using SharedKernal;

namespace OrderApi.ComponentTests.Application.Infrastructure;

public class MessageBusMock : IBus, IMessageSource
{
    public event Action<object> OnMessage;

    public Task Publish<TMessage>(TMessage message)
    {
        OnMessage?.Invoke(message);
        return Task.CompletedTask;
    }
}
