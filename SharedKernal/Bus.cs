namespace SharedKernal;

public interface IBus
{
    Task Publish<TMessage>(TMessage message);
}
