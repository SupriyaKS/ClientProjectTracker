using Microsoft.EntityFrameworkCore;
using ClientProjectTracker.Data;
using ClientProjectTracker.Models;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TaskItem>> GetByProjectIdAsync(int projectId)
    {
        return await _context.TaskItems
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task CreateAsync(TaskItem task)
    {
        _context.TaskItems.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.TaskItems.CountAsync();
    }

    public async Task<int> GetCompletedCountAsync()
    {
        return await _context.TaskItems
            .CountAsync(x => x.Status == "Completed");
    }
    public async Task<int> GetPendingCountAsync()
    {

        return await _context.TaskItems
            .CountAsync(x => x.Status == "Pending");

    }

    public async Task<int> GetInProgressCountAsync()
    {

        return await _context.TaskItems
            .CountAsync(x => x.Status == "In Progress");

    }

    public async Task<List<TaskItem>> GetAllAsync()
    {
        return await _context.TaskItems
        .Include(t => t.Project)
        .ToListAsync();
    }
}