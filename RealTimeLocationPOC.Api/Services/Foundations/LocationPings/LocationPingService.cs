using RealTimeLocationPOC.Api.Brokers.Storages;
using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Services.Foundations.LocationPings
{
    public class LocationPingService : ILocationPingService
    {
        private readonly IStorageBroker storageBroker;

        public LocationPingService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async ValueTask<LocationPing> AddLocationPingAsync(LocationPing locationPing) =>
            await this.storageBroker.InsertLocationPingAsync(locationPing);

        public IQueryable<LocationPing> RetrieveAllLocationPings() =>
            this.storageBroker.SelectAllLocationPings();
    }
}
