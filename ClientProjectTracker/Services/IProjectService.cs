using ClientProjectTracker.Models;

public interface IProjectService
{
    Task<List<Project>> GetByClientIdAsync(int clientId);
    Task CreateAsync(Project project);
    Task<int> GetTotalCountAsync();
}