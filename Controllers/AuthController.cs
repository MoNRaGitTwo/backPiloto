using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using DemoPilotoV1.DTOS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DemoPilotoV1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly BaseDeDatos _context;

        public AuthController(BaseDeDatos context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] Users newUser)
        {
            try
            {
                // Verifica si el usuario ya existe
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Nombre == newUser.Nombre);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "El usuario ya existe" });
                }

                // Agrega el nuevo usuario
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuario registrado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error al registrar usuario: {ex.Message}" });
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Nombre == loginUser.Nombre && u.Password == loginUser.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Usuario o contraseña inválidos" });
            }

            return Ok(new
            {
                id = user.Id,
                nombre = user.Nombre,
                direccion = user.Direccion,
                telefono = user.Telefono,
                deuda = user.Deuda,
                message = "Inicio de sesión exitoso"
            });
        }
    }
}
