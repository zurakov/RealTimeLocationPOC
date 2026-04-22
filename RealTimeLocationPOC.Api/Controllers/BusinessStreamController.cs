using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RealTimeLocationPOC.Api.Models;
using RealTimeLocationPOC.Api.Services.Foundations.SseChannels;
using RealTimeLocationPOC.Api.Services.Orchestrations;

namespace RealTimeLocationPOC.Api.Controllers
{
    [ApiController]
    [Route("api/businesses")]
    public class BusinessStreamController : ControllerBase
    {
        private readonly ILocationOrchestrationService locationOrchestrationService;
        private readonly SseChannelService sseChannelService;

        public BusinessStreamController(
            ILocationOrchestrationService locationOrchestrationService,
            SseChannelService sseChannelService)
        {
            this.locationOrchestrationService = locationOrchestrationService;
            this.sseChannelService = sseChannelService;
        }

        [HttpGet("{businessId}/stream")]
        public async Task StreamAsync(Guid businessId, CancellationToken cancellationToken)
        {
            await this.locationOrchestrationService.StreamLocationUpdatesAsync(
                businessId, Response, cancellationToken);
        }
    }
}
