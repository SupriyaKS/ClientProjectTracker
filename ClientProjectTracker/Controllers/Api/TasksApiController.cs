using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using ClientProjectTracker.Models;
using Microsoft.AspNetCore.Authorization;
using ClientProjectTracker.DTOs;
using ClientProjectTracker.Data;

namespace ClientProjectTracker.Controllers.Api
{
    [Authorize]
    [Route("api/tasks")]
    [ApiController]
    public class TasksApiController : ControllerBase
    {
        private readonly ITaskService _service;
        private readonly ApplicationDbContext _context;

        public TasksApiController(ITaskService service, ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet("{projectId}")]
        public async Task<IActionResult> Get(int projectId) //api/tasks/5
        {
            var tasks = await _service.GetByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpPut("{id}/status")] // api/tasks/5/status
        public async Task<IActionResult> UpdateStatus(
            int id,
            [FromBody] UpdateTaskStatusDto dto)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)

                return NotFound();

            task.Status = dto.Status;

            await _context.SaveChangesAsync();

            return Ok(task);
        }
    }
}