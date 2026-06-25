using Microsoft.EntityFrameworkCore;
using ClientProjectTracker.Data;
using ClientProjectTracker.Models;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Project>> GetByClientIdAsync(int clientId)
    {
        return await _context.Projects
            .Where(p => p.ClientId == clientId)
            .ToListAsync();
    }

    public async Task CreateAsync(Project project)
    {
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetTotalCountAsync()
    {
        return await _context.Projects.CountAsync();
    }

    public async Task<List<Project>> GetAllAsync()
    {
        return await _context.Projects
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
}