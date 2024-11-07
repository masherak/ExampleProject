using System.Text.Json;
using ExampleProject.Serializers;
using ExampleProject.Stores;

namespace ExampleProject.Jobs;

public class OrderJob(ILogger<OrderJob> logger)
{
    public async Task CommitBufferedOrdersAsync()
    {
        var orderDtos = OrderStore.FlushOrders();

        if (!orderDtos.Any())
        {
            return;
        }
        
        try
        {
            foreach (var orderDto in orderDtos)
            {
                // TODO: Implement sending data to the internal system
                await Task.Delay(TimeSpan.Zero);
                
                logger.LogInformation("Order commited: {Order}", JsonSerializer.Serialize(orderDto, AppJsonSerializerContext.Default.OrderDto));
            }
        }
        catch (Exception ex)
        {
            var serializedOrders = JsonSerializer.Serialize(orderDtos, AppJsonSerializerContext.Default.IEnumerableOrderDto);
            
            logger.LogCritical(ex, "An error occurred commiting buffered orders - {Orders}", serializedOrders);
            
            throw;
        }
    }
}
