using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using ClientProjectTracker.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IClientService _clientService;

    public ProjectsController(IProjectService projectService, IClientService clientService)
    {
        _projectService = projectService;
        _clientService = clientService;
    }

    public async Task<IActionResult> Index(int clientId)
    {
        if(clientId == 0)
        {
           clientId = (await _clientService.GetAllAsync()).FirstOrDefault()?.Id ?? 0;
        }
        var projects = await _projectService.GetByClientIdAsync(clientId);
        ViewBag.ClientId = clientId;
        return View(projects);
    }

    public async Task<IActionResult> CreateAsync(int clientId)
    {

        var clients = await _clientService.GetAllAsync();

        Console.WriteLine($"Clients Count: {clients.Count()}");

        ViewBag.Clients = new SelectList(

            clients,

            "Id",

            "Name");
        /* ViewBag.Clients =

        new SelectList(

            await _clientService.GetAllAsync(),

            "Id",

            "Name");*/

        if(clientId != 0)
        {
            ViewBag.ClientId = clientId;
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Project project)
    {
        try{
            await _projectService.CreateAsync(project);
             return RedirectToAction("Index", new { clientId = project.ClientId });

        }

        catch (Exception ex)
        {
            return Content(
                ex.InnerException?.Message ?? ex.Message);
            }
        }
}