using System.Collections.Concurrent;
using ExampleProject.Dtos;

namespace ExampleProject.Stores;

public static class OrderStore
{
    private static ConcurrentDictionary<int, int> _orders = new();
    
    public static int Count => _orders.Count;

    public static void AddOrUpdate(OrderDto order)
    {
        _orders.AddOrUpdate(order.ProductId, order.Quantity, (_, quantity) => quantity + order.Quantity);
    }

    public static ICollection<OrderDto> FlushOrders()
    {
        // Note: Identify and describe any potential weaknesses or limitations of the chosen approach. Synchronization options include using a semaphore, lock, or other synchronization methods.
        return Interlocked.Exchange(ref _orders, new ConcurrentDictionary<int, int>())
            .Select(_ => new OrderDto(_.Key, _.Value))
            .ToList();
    }
}