namespace OrderApi.Core;

public interface IOrderIdGenerator
{
    Guid New();
}

public class OrderIdGenerator : IOrderIdGenerator
{
    public Guid New() => Guid.NewGuid();
}
