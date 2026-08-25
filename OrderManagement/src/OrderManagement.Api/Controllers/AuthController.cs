using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("auth")]
public sealed class AuthController(IConfiguration config) : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login(LoginRequest request)
    {
        if (request.Email != "dev@martech.com" || request.Password != "Senha@123")
        {
            return Unauthorized();
        }

        DateTime now = DateTime.UtcNow;
        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, request.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        IConfigurationSection jwtSettings = config.GetSection("Jwt");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

        var token = new JwtSecurityToken(jwtSettings["Issuer"], jwtSettings["Audience"], claims, now, now.AddHours(1),
                                         new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            expiresIn = 3600
        });
    }
}