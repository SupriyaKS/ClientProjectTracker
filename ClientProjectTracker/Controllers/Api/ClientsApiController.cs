using Microsoft.AspNetCore.Mvc;
using ClientProjectTracker.Services;
using ClientProjectTracker.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;



namespace ClientProjectTracker.Controllers.Api
{
    [ApiController]
    [Route("api/clients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    
    public class ClientsApiController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsApiController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // GET: api/clients
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var result = await _clientService.GetClientsAsync(
                userId: "",   // for now skip auth
                isAdmin: true,
                search: null,
                page: 1,
                pageSize: 100);
                return Ok(new {
                    success = true,
                    data = result.Clients
                });

            //return Ok(result.Clients);
        }

        // GET: api/clients/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _clientService.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        public async Task<IActionResult> Create(ClientDto dto)
        {
            await _clientService.CreateAsync(dto, "api-user");

            return Ok(new { message = "Client created" });
        }

        // DELETE: api/clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteAsync(id);
            return Ok();
        }
        
    }
}