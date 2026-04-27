using Microsoft.AspNetCore.Mvc;
using RealTimeLocationPOC.Api.Models;
using RealTimeLocationPOC.Api.Services.Orchestrations;

namespace RealTimeLocationPOC.Api.Controllers
{
    [ApiController]
    [Route("api/locationpings")]
    public class LocationPingsController : ControllerBase
    {
        private readonly ILocationOrchestrationService locationOrchestrationService;

        public LocationPingsController(ILocationOrchestrationService locationOrchestrationService)
        {
            this.locationOrchestrationService = locationOrchestrationService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<LocationPing>> PostLocationPingAsync(LocationPing locationPing)
        {
            LocationPing processedPing =
                await this.locationOrchestrationService.ProcessLocationPingAsync(locationPing);

            return Ok(processedPing);
        }

        [HttpPost("offline/{employeeId}")]
        public async ValueTask<ActionResult> PostEmployeeOfflineAsync(
            Guid employeeId)
        {
            await this.locationOrchestrationService
                .MarkEmployeeOfflineAsync(
                    employeeId);

            return Ok();
        }
    }
}
