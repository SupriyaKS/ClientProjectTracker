using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientProjectTracker.Data;
using ClientProjectTracker.Models;
using ClientProjectTracker.DTOs;
using ClientProjectTracker.Services;
using Microsoft.AspNetCore.Identity;   
using ClosedXML.Excel;

namespace ClientProjectTracker.Controllers
{
    [Authorize]
    public class ClientsController : Controller
    {
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;

        public ClientsController(IClientService clientService, UserManager<IdentityUser> userManager, IProjectService projectService, ITaskService taskService)
        {
            _clientService = clientService;
            _userManager = userManager;
            _projectService = projectService;
            _taskService = taskService;
        }
        // ================= CREATE =================

        // GET: Clients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

           var user = await _userManager.GetUserAsync(User);

            await _clientService.CreateAsync(dto, user!.Id);

            return RedirectToAction(nameof(Index));
        }
        // ================= EDIT =================

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            var dto = new ClientDto
            {
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                CompanyName = client.CompanyName
            };

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);
            await _clientService.UpdateAsync(id, dto);

            return RedirectToAction(nameof(Index));
        }

         // ================= DELETE =================
         [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // ================= INDEX =================

       public async Task<IActionResult> Index(string? search, int page = 1)
        {
            int pageSize = 5;

            var user = await _userManager.GetUserAsync(User);

            var result = await _clientService.GetClientsAsync(
                            user!.Id,
                            User.IsInRole("Admin"),
                            search,
                            page,
                            pageSize);


            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize);
            ViewBag.Search = search;

            return View(result.Clients);
        }

        // ================= DASHBOARD =================
        [Authorize]
        public async Task<IActionResult> Dashboard()

        {

            var totalClients = await _clientService.GetTotalCountAsync();

            var totalProjects = await _projectService.GetTotalCountAsync();

            var totalTasks = await _taskService.GetTotalCountAsync();

            var completedTasks = await _taskService.GetCompletedCountAsync();

            var recentClients = await _clientService.GetRecentAsync(totalClients);

            var allClients = await _clientService.GetRecentAsync(1000);
            

            var groupedData = allClients

                .GroupBy(c => c.CreatedAt.Date)

                .Select(g => new

                {

                    Date = g.Key.ToString("yyyy-MM-dd"),

                    Count = g.Count()

                })

                .OrderBy(x => x.Date)

                .ToList();

            ViewBag.TotalClients = totalClients;

            ViewBag.TotalProjects = totalProjects;

            ViewBag.TotalTasks = totalTasks;

            ViewBag.CompletedTasks = completedTasks;

            ViewBag.ChartLabels = groupedData.Select(x => x.Date).ToList();

            ViewBag.ChartData = groupedData.Select(x => x.Count).ToList();

            ViewBag.PendingTasks = await _taskService.GetPendingCountAsync();
            ViewBag.InProgressTasks = await _taskService.GetInProgressCountAsync();

            return View(recentClients);

        }

        public async Task<IActionResult> ExportToExcel()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            var result = await _clientService.GetClientsAsync(
                user.Id,
                User.IsInRole("Admin"),
                null,
                1,
                1000); // export all

            var clients = result.Clients;

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Clients");

            // Header
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Phone";
            worksheet.Cell(1, 4).Value = "Company";

            // Data
            for (int i = 0; i < clients.Count; i++)
            {
                var c = clients[i];
                worksheet.Cell(i + 2, 1).Value = c.Name;
                worksheet.Cell(i + 2, 2).Value = c.Email;
                worksheet.Cell(i + 2, 3).Value = c.Phone;
                worksheet.Cell(i + 2, 4).Value = c.CompanyName;
            }
            var headerRow = worksheet.Range("A1:D1");
            headerRow.Style.Font.Bold = true;
            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Clients.xlsx");
        }

    }
}