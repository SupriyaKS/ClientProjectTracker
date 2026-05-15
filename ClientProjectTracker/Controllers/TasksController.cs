using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using ClientProjectTracker.Models;

public class TasksController : Controller
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<IActionResult> Index(int projectId)
    {
        var tasks = await _taskService.GetByProjectIdAsync(projectId);
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }

    public IActionResult Create(int projectId)
    {
        ViewBag.ProjectId = projectId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        await _taskService.CreateAsync(task);
        return RedirectToAction("Index", new { projectId = task.ProjectId });
    }

    public async Task<IActionResult> Kanban()
    {
        var tasks = await _taskService.GetAllAsync();
        return View(tasks);
    }
}