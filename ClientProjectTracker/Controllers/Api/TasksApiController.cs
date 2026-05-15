using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using ClientProjectTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace ClientProjectTracker.Controllers.Api
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService _service;

        public TasksApiController(ITaskService service)
        {
            _service = service;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> Get(int projectId)
        {
            var tasks = await _service.GetByProjectIdAsync(projectId);
            return Ok(tasks);
        }
    }
}