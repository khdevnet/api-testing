using SharedKernal;

namespace OrderApi.Infrastructure.ExternalServices;
internal class AzureServiceBus : IBus
{
    /// <summary>
    /// Your real implementation could be there.
    /// </summary>
    public Task Publish<TMessage>(TMessage message) => throw new NotImplementedException();
}
