using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Coin.Models;
using Coin.Data;  // Necesario para CoinDbContext
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly CoinDbContext _context;

        public AuthController(IConfiguration configuration, CoinDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }



        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginData)
        {
            // Buscar el usuario en la base de datos
            var usuario = _context.COIN_Usuarios.FirstOrDefault(u => u.Correo == loginData.Correo && u.ContrasenaHash == loginData.ContrasenaHash);

            if (usuario == null)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(usuario);
            return Ok(new { token });
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()), // Usar IdUsuario como principal
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.Role, usuario.Rol)  // Añadir el rol al token
    };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
