using ExampleProject.Dtos;
using ExampleProject.Stores;

namespace TestProject;

public class OrderStoreTests
{
    private readonly ICollection<OrderDto> _orders =
    [
        new(1, 1),
        new(1, 2),
        new(2, 3),
        new(3, 4),
        new(4, 5),
        new(4, 5)
    ];
    
    [Fact]
    public void Test()
    {
        foreach (var order in _orders)
        {
            OrderStore.AddOrUpdate(order);
        }
        
        Assert.False(OrderStore.Count == _orders.Count, "Order count is incorrect");

        var orders = OrderStore.FlushOrders();
        
        Assert.True(OrderStore.Count == default, "Expected order count to be zero.");

        var groupedOrders = _orders
            .GroupBy(_ => _.ProductId)
            .Select(g => new OrderDto(g.Key, g.Sum(o => o.Quantity)))
            .ToList();
        
        Assert.True(groupedOrders.Count == orders.Count, "Expected order count to match.");
        
        Assert.True(groupedOrders.All(go => orders.Any(o => o.ProductId == go.ProductId && o.Quantity == go.Quantity)));
    }
}