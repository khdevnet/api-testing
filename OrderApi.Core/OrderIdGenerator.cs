namespace OrderApi.Core;

public class OrderIdGenerator : IOrderIdGenerator
{
    public Guid New() => Guid.NewGuid();
}

public interface IOrderIdGenerator
{
    Guid New();
}
