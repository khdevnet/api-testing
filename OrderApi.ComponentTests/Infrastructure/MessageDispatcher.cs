﻿using System;
using System.Threading.Tasks;
using LightBDD.Framework.Messaging;
using Rebus.Handlers;

namespace OrderApi.ComponentTests.Infrastructure;

internal class MessageDispatcher : IMessageSource, IHandleMessages<object>
{
    public event Action<object> OnMessage;

    public Task Handle(object message)
    {
        OnMessage?.Invoke(message);
        return Task.CompletedTask;
    }
}
