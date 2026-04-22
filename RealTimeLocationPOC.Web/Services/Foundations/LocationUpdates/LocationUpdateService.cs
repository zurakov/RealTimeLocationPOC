using System.Text.Json;
using RealTimeLocationPOC.Web.Brokers.BroadCasts;
using RealTimeLocationPOC.Web.Models;

namespace RealTimeLocationPOC.Web.Services.Foundations.LocationUpdates
{
    public class LocationUpdateService : ILocationUpdateService
    {
        private readonly IBroadCastBroker broadCastBroker;
        private CancellationTokenSource? cts;

        public event Action<LocationUpdate>? OnLocationUpdateReceived;

        public LocationUpdateService(IBroadCastBroker broadCastBroker)
        {
            this.broadCastBroker = broadCastBroker;
        }

        public async ValueTask StartListeningAsync(Guid businessId)
        {
            this.cts?.Cancel();
            this.cts = new CancellationTokenSource();

            _ = Task.Run(async () =>
            {
                try
                {
                    using Stream stream = await this.broadCastBroker.GetRawStreamAsync(businessId);
                    using var reader = new StreamReader(stream);

                    while (!this.cts.Token.IsCancellationRequested && !reader.EndOfStream)
                    {
                        string? line = await reader.ReadLineAsync(this.cts.Token);

                        if (!string.IsNullOrWhiteSpace(line) && line.StartsWith("data: "))
                        {
                            string jsonPayload = line.Substring(6).Trim();

                            var location = JsonSerializer.Deserialize<LocationUpdate>(jsonPayload, new JsonSerializerOptions 
                            { 
                                PropertyNameCaseInsensitive = true 
                            });

                            if (location != null)
                            {
                                this.OnLocationUpdateReceived?.Invoke(location);
                            }
                        }
                    }
                }
                catch (OperationCanceledException) { }
                catch (Exception ex)
                {
                    Console.WriteLine($"Stream error: {ex.Message}");
                }
            }, this.cts.Token);
        }

        public ValueTask StopListeningAsync(Guid businessId)
        {
            this.cts?.Cancel();
            return ValueTask.CompletedTask;
        }
    }
}
