using RealTimeLocationPOC.Web.Models;

namespace RealTimeLocationPOC.Web.Services.Foundations.LocationUpdates
{
    public interface ILocationUpdateService
    {
        event Action<LocationUpdate> OnLocationUpdateReceived;
        ValueTask StartListeningAsync(Guid businessId);
        ValueTask StopListeningAsync(Guid businessId);
    }
}
