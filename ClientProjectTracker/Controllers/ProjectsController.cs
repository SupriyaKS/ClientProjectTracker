using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using ClientProjectTracker.Models;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public async Task<IActionResult> Index(int clientId)
    {
        var projects = await _projectService.GetByClientIdAsync(clientId);
        ViewBag.ClientId = clientId;
        return View(projects);
    }

    public IActionResult Create(int clientId)
    {
        ViewBag.ClientId = clientId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Project project)
    {
        await _projectService.CreateAsync(project);
        return RedirectToAction("Index", new { clientId = project.ClientId });
    }
}