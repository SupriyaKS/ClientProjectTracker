using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using ClientProjectTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class TasksController : Controller
{
    private readonly ITaskService _taskService;
    private readonly IProjectService _projectService;

    public TasksController(ITaskService taskService, IProjectService projectService)
    {
        _taskService = taskService;
        _projectService = projectService;
    }

    public async Task<IActionResult> Index(int projectId)
    {
        if(projectId == 0)
        {
           projectId = (await _projectService.GetAllAsync()).FirstOrDefault()?.Id ?? 0;
        }
        var tasks = await _taskService.GetByProjectIdAsync(projectId);
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }

    public async Task<IActionResult> CreateAsync(int projectId)
    {

        ViewBag.Projects =

    new SelectList(

        await _projectService.GetAllAsync(),

        "Id",

        "Name");
        ViewBag.ProjectId = projectId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        //await _taskService.CreateAsync(task);
        // return RedirectToAction("Index", new { projectId = task.ProjectId });

        try

        {

            await _taskService.CreateAsync(task);
            return RedirectToAction("Index", new { projectId = task.ProjectId });

        }

        catch (Exception ex)
        {
            return Content(
                ex.InnerException?.Message ?? ex.Message);
        }
    }

    public async Task<IActionResult> Kanban()
    {
        var tasks = await _taskService.GetAllAsync();
        return View(tasks);
    }
}