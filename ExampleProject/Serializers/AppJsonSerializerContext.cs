using System.Text.Json.Serialization;
using ExampleProject.Dtos;

namespace ExampleProject.Serializers;

// AOT requirement
[JsonSerializable(typeof(OrderDto))]
[JsonSerializable(typeof(List<OrderDto>))]
[JsonSerializable(typeof(IEnumerable<OrderDto>))]
[JsonSerializable(typeof(IAsyncEnumerable<OrderDto>))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
