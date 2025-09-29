using DesafioFast.Models;
using DesafioFast.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DesafioFast.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class LoginController : ControllerBase
    {
        private readonly TokenService _tokenService;

        private readonly List<UsuarioModels> usuarios = new()
    {
        new UsuarioModels { Id = 1, Nome = "Admin", Email = "admin@teste.com", Senha = "1234", Role = "Admin" },
        new UsuarioModels { Id = 2, Nome = "Usuario", Email = "user@teste.com", Senha = "1234", Role = "Usuario" }
    };


        public LoginController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] UsuarioModels login)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Email == login.Email && u.Senha == login.Senha);
            if (usuario == null)
            {
                return Unauthorized(new { message = "Credenciais inválidas" });
            }
            var token = _tokenService.GerarToken(usuario);
            return Ok(new { token });
        }
        [HttpGet("me")]
        [Authorize]
        public IActionResult Me()
        {
            var nome = User.FindFirst(ClaimTypes.Name)?.Value;
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            return Ok(new { nome, email, role });
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Allyson";
        
        [HttpGet]
        [Route("autenticado")]
        [Authorize]
        public string Autenticado() => $"Autenticado - {User.Identity.Name}";
    }
}
