using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Services.Foundations.LocationPings
{
    public interface ILocationPingService
    {
        ValueTask<LocationPing> AddLocationPingAsync(LocationPing locationPing);
        IQueryable<LocationPing> RetrieveAllLocationPings();
    }
}
