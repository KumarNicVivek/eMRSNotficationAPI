using Microsoft.AspNetCore.Mvc;
using SERVICEAPP.ServiceLayer;
using Microsoft.AspNetCore.SignalR;
using emrsCandidateService.SignalHub;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace emrsCandidateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        private readonly IVisitorService _visitorService;
        private readonly IHubContext<VisitorHub> _hubContext;

        public VisitorController(IVisitorService visitorService, IHubContext<VisitorHub> hubContext)
        {
            _visitorService = visitorService;
            _hubContext = hubContext;
        }

        //[HttpGet("count")]
        //public async Task<IActionResult> GetVisitorCount()
        //{
        //    var count = await _visitorService.TrackVisitorAsync(HttpContext);
        //    return Ok(new { totalVisitors = count });
        //}

        [HttpGet("count")]
        public async Task<IActionResult> GetVisitorCount()
        {
            var count = await _visitorService.TrackVisitorAsync(HttpContext);


            // Broadcast to all clients
            await _hubContext.Clients.All.SendAsync("ReceiveVisitorCount", count);

            return Ok(new { totalVisitors = count });
        }
    }
}
