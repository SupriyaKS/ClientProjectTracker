using ClientProjectTracker.Models;

public interface ITaskService
{
    Task<List<TaskItem>> GetByProjectIdAsync(int projectId);
    Task CreateAsync(TaskItem task);

    Task<int> GetTotalCountAsync();
    Task<int> GetCompletedCountAsync();
    Task<int> GetPendingCountAsync();
    Task<int> GetInProgressCountAsync();
    Task<List<TaskItem>> GetAllAsync();
}