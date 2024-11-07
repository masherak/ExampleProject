using ExampleProject.Dtos;
using ExampleProject.Jobs;
using ExampleProject.Serializers;
using ExampleProject.Stores;
using Hangfire;
using Hangfire.MemoryStorage;

// Attempt to use AOT compilation with Minimal API in this project
var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(default, AppJsonSerializerContext.Default);
});

builder.Services.AddSingleton<OrderJob>();

builder.Services.AddHangfire(_ => _.UseMemoryStorage());

builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseHangfireDashboard();

// TODO: Validation - productId == default || quantity == default?
app.MapPost("/order", async (IAsyncEnumerable<OrderDto> orders) =>
{
    await foreach (var order in orders)
    {
        OrderStore.AddOrUpdate(order);
    }
});

RecurringJob.AddOrUpdate<OrderJob>("move-orders-to-database-job", _ => _.CommitBufferedOrdersAsync(), Cron.Minutely);

await app.RunAsync();
