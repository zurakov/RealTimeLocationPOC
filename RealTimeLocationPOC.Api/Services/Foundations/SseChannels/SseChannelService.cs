using System.Collections.Concurrent;
using System.Threading.Channels;
using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Services.Foundations.SseChannels
{
    public class SseChannelService
    {
        private readonly ConcurrentDictionary<Guid, Channel<EmployeeLocation>> channels = new();

        public ChannelReader<EmployeeLocation> GetReader(Guid businessId)
        {
            var channel = this.channels.GetOrAdd(businessId,
                _ => Channel.CreateUnbounded<EmployeeLocation>());

            return channel.Reader;
        }

        public async ValueTask PublishAsync(Guid businessId, EmployeeLocation location)
        {
            var channel = this.channels.GetOrAdd(businessId,
                _ => Channel.CreateUnbounded<EmployeeLocation>());

            await channel.Writer.WriteAsync(location);
        }
    }
}
