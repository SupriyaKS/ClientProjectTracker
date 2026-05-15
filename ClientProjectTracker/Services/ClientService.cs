using Microsoft.EntityFrameworkCore;
using ClientProjectTracker.Data;
using ClientProjectTracker.DTOs;
using ClientProjectTracker.Models;

namespace ClientProjectTracker.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Client>, int)> 
            GetClientsAsync(string userId, bool isAdmin, string? search, int page, int pageSize)
        {
            var query = _context.Clients.AsQueryable();

            if (!isAdmin)
                query = query.Where(c => c.OwnerId == userId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Name.Contains(search) ||
                    c.Email.Contains(search) ||
                    c.CompanyName!.Contains(search));
            }

            var total = await query.CountAsync();

            var clients = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (clients, total);
        }

        public async Task<Client?> GetByIdAsync(int id)
            => await _context.Clients.FindAsync(id);

        public async Task CreateAsync(ClientDto dto, string userId)
        {
            var client = new Client
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CompanyName = dto.CompanyName,
                OwnerId = userId
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return;

            client.Name = dto.Name;
            client.Email = dto.Email;
            client.Phone = dto.Phone;
            client.CompanyName = dto.CompanyName;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return;

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetTotalCountAsync()
            => await _context.Clients.CountAsync();

        public async Task<List<Client>> GetRecentAsync(int count)
            => await _context.Clients
                .OrderByDescending(c => c.CreatedAt)
                .Take(count)
                .ToListAsync();
    }
}