using ClientProjectTracker.DTOs;
using ClientProjectTracker.Models;

namespace ClientProjectTracker.Services
{
    public interface IClientService
    {
        Task<(List<Client> Clients, int TotalCount)> 
            GetClientsAsync(string userId, bool isAdmin, string? search, int page, int pageSize);

        Task<Client?> GetByIdAsync(int id);

        Task CreateAsync(ClientDto dto, string userId);

        Task UpdateAsync(int id, ClientDto dto);

        Task DeleteAsync(int id);

        Task<int> GetTotalCountAsync();
        Task<List<Client>> GetRecentAsync(int count);
    }
}