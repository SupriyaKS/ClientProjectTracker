using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using ClientProjectTracker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ClientProjectTracker.DTOs;

namespace ClientProjectTracker.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration config,
    ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized();

            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                UserId = user.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(DTOs.TokenRequestDto request)
        {
            var storedToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

        if (user == null)
            return Unauthorized();

        var newAccessToken = GenerateJwtToken(user);

        return Ok(new
        {
            accessToken = newAccessToken
        });
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutDto request)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

            if (token == null)
                return BadRequest("Invalid token");

            // ✅ Revoke token
            token.IsRevoked = true;

            await _context.SaveChangesAsync();

            return Ok("Logged out successfully");
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginDto
    {
        
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}