using System.Threading.Channels;
using Microsoft.EntityFrameworkCore;
using RealTimeLocationPOC.Api.Brokers.DateTimes;
using RealTimeLocationPOC.Api.Brokers.Loggings;
using RealTimeLocationPOC.Api.Models;
using RealTimeLocationPOC.Api.Services.Foundations.Employees;
using RealTimeLocationPOC.Api.Services.Foundations.LocationPings;
using RealTimeLocationPOC.Api.Services.Foundations.SseChannels;

namespace RealTimeLocationPOC.Api.Services.Orchestrations
{
    public class LocationOrchestrationService : ILocationOrchestrationService
    {
        private readonly IEmployeeService employeeService;
        private readonly ILocationPingService locationPingService;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly SseChannelService sseChannelService;

        public LocationOrchestrationService(
            IEmployeeService employeeService,
            ILocationPingService locationPingService,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker,
            SseChannelService sseChannelService)
        {
            this.employeeService = employeeService;
            this.locationPingService = locationPingService;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
            this.sseChannelService = sseChannelService;
        }

        public async ValueTask<LocationPing> ProcessLocationPingAsync(LocationPing locationPing)
        {
            locationPing.Id = Guid.NewGuid();
            locationPing.RecordedAt = this.dateTimeBroker.GetCurrentDateTimeOffset();

            LocationPing storedPing =
                await this.locationPingService.AddLocationPingAsync(locationPing);

            Employee employee =
                await this.employeeService.RetrieveEmployeeByIdAsync(locationPing.EmployeeId);

            employee.IsOnline = true;
            employee.LastSeenAt = storedPing.RecordedAt;

            await this.employeeService.ModifyEmployeeAsync(employee);

            var employeeLocation = new EmployeeLocation
            {
                EmployeeId = employee.Id,
                FullName = employee.FullName,
                Latitude = storedPing.Latitude,
                Longitude = storedPing.Longitude,
                Heading = storedPing.Heading,
                IsOnline = true,
                LastSeenAt = storedPing.RecordedAt
            };

            await this.sseChannelService.PublishAsync(employee.BusinessId, employeeLocation);

            this.loggingBroker.LogInformation(
                $"Processed ping for employee {employee.FullName} at ({storedPing.Latitude}, {storedPing.Longitude})");

            return storedPing;
        }

        public async ValueTask MarkEmployeeOfflineAsync(Guid employeeId)
        {
            Employee employee =
                await this.employeeService.RetrieveEmployeeByIdAsync(employeeId);

            employee.IsOnline = false;

            await this.employeeService.ModifyEmployeeAsync(employee);

            var employeeLocation = new EmployeeLocation
            {
                EmployeeId = employee.Id,
                FullName = employee.FullName,
                Latitude = 0,
                Longitude = 0,
                Heading = null,
                IsOnline = false,
                LastSeenAt = employee.LastSeenAt ?? this.dateTimeBroker.GetCurrentDateTimeOffset()
            };

            await this.sseChannelService.PublishAsync(employee.BusinessId, employeeLocation);

            this.loggingBroker.LogInformation(
                $"Employee {employee.FullName} marked offline");
        }

        public async ValueTask<List<EmployeeLocation>> RetrieveBusinessSnapshotAsync(Guid businessId)
        {
            List<Employee> employees = await this.employeeService
                .RetrieveAllEmployees()
                .Where(e => e.BusinessId == businessId)
                .ToListAsync();

            IQueryable<LocationPing> allPings = this.locationPingService.RetrieveAllLocationPings();

            var employeeLocations = new List<EmployeeLocation>();

            foreach (Employee employee in employees)
            {
                LocationPing latestPing = await allPings
                    .Where(p => p.EmployeeId == employee.Id)
                    .OrderByDescending(p => p.RecordedAt)
                    .FirstOrDefaultAsync();

                var employeeLocation = new EmployeeLocation
                {
                    EmployeeId = employee.Id,
                    FullName = employee.FullName,
                    Latitude = latestPing?.Latitude ?? 0,
                    Longitude = latestPing?.Longitude ?? 0,
                    Heading = latestPing?.Heading,
                    IsOnline = employee.IsOnline,
                    LastSeenAt = employee.LastSeenAt ?? DateTimeOffset.MinValue
                };


                employeeLocations.Add(employeeLocation);
            }

            return employeeLocations;
        }

        public async Task StreamLocationUpdatesAsync(
            Guid businessId,
            HttpResponse response,
            CancellationToken cancellationToken)
        {
            response.ContentType = "text/event-stream";
            response.Headers.Append("Cache-Control", "no-cache");
            response.Headers.Append("Connection", "keep-alive");
            List<EmployeeLocation> snapshot = await this.RetrieveBusinessSnapshotAsync(businessId);
            foreach (var location in snapshot)
            {
                string snapshotJson = System.Text.Json.JsonSerializer.Serialize(location);
                await response.WriteAsync($"data: {snapshotJson}\n\n", cancellationToken);
            }
            await response.Body.FlushAsync(cancellationToken);
            ChannelReader<EmployeeLocation> reader = this.sseChannelService.GetReader(businessId);

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    EmployeeLocation location = await reader.ReadAsync(cancellationToken);
                    string locationJson = System.Text.Json.JsonSerializer.Serialize(location);
                    await response.WriteAsync($"data: {locationJson}\n\n", cancellationToken);
                    await response.Body.FlushAsync(cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }
    }
}
