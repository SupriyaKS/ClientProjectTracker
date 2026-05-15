using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using Microsoft.AspNetCore.Authorization;

namespace ClientProjectTracker.Controllers.Api
{
    [Authorize]
    [Route("api/projects")]
    [ApiController]
    public class ProjectsApiController : ControllerBase
    {
        private readonly IProjectService _service;

        public ProjectsApiController(IProjectService service)
        {
            _service = service;
        }

        [HttpGet("{clientId}")]
        public async Task<IActionResult> Get(int clientId)
        {
            var projects = await _service.GetByClientIdAsync(clientId);
            return Ok(projects);
        }
    }
}