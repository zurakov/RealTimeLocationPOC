using RealTimeLocationPOC.Api.Brokers.DateTimes;
using RealTimeLocationPOC.Api.Brokers.Loggings;
using RealTimeLocationPOC.Api.Brokers.Storages;
using RealTimeLocationPOC.Api.Services.Foundations.Employees;
using RealTimeLocationPOC.Api.Services.Foundations.LocationPings;
using RealTimeLocationPOC.Api.Services.Foundations.SseChannels;
using RealTimeLocationPOC.Api.Services.Orchestrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddTransient<IStorageBroker, StorageBroker>();
builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<ILocationPingService, LocationPingService>();
builder.Services.AddTransient<ILocationOrchestrationService, LocationOrchestrationService>();
builder.Services.AddSingleton<SseChannelService>();
var app = builder.Build();
app.UseCors(policyName: "AllowAll");
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var storageBroker = scope.ServiceProvider.GetRequiredService<IStorageBroker>();
}

app.Run();
