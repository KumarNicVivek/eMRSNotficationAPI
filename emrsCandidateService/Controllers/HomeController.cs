using Microsoft.AspNetCore.Mvc;
using SERVICEAPP.ServiceLayer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace emrsCandidateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IVisitorService _visitorService;

        public HomeController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetVisitorCount()
        {
            var count = await _visitorService.TrackVisitorAsync(HttpContext);
            return Ok(new { totalVisitors = count });
        }
    }
}
