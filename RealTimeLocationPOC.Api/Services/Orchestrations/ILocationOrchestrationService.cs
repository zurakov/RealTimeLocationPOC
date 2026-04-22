using RealTimeLocationPOC.Api.Models;

namespace RealTimeLocationPOC.Api.Services.Orchestrations
{
    public interface ILocationOrchestrationService
    {
        ValueTask<LocationPing> ProcessLocationPingAsync(LocationPing locationPing);
        ValueTask MarkEmployeeOfflineAsync(Guid employeeId);
        ValueTask<List<EmployeeLocation>> RetrieveBusinessSnapshotAsync(Guid businessId);
        Task StreamLocationUpdatesAsync(Guid businessId, Microsoft.AspNetCore.Http.HttpResponse response, CancellationToken cancellationToken);
    }
}
