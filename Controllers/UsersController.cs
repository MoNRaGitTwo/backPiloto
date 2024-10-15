using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using DemoPilotoV1.DTOs;
using DemoPilotoV1.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DemoPilotoV1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly BaseDeDatos _context;
        private readonly UsersRepository _usersRepository;

        public UsersController(BaseDeDatos context, UsersRepository usersRepository)
        {
            _context = context;
            _usersRepository = usersRepository; // Asignar el repositorio inyectado
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            var user = new Users
            {
                Nombre = userDto.Nombre,
                Password = userDto.Password,
                Direccion = userDto.Direccion,
                Telefono = userDto.Telefono,
                Deuda = userDto.Deuda
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuario registrado exitosamente" });
        }

        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            // Convertir la lista de usuarios a una lista de DTOs incluyendo el campo Id
            var usersDto = users.Select(u => new UserDto
            {
                Id = u.Id,  // Asegúrate de incluir el Id aquí
                Nombre = u.Nombre,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Deuda = u.Deuda
            }).ToList();

            return Ok(usersDto);
        }

        // Nueva ruta para actualizar la deuda
        [HttpPut("{id}/actualizarDeuda")]
        [SwaggerOperation("Actualiza la deuda de un cliente")]
        public async Task<IActionResult> ActualizarDeuda(int id, [FromBody] decimal nuevaDeuda)
        {
            var result = await _usersRepository.ActualizarDeudaAsync(id, nuevaDeuda);

            if (!result)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(new { message = "Deuda actualizada exitosamente", deudaActualizada = nuevaDeuda });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Nombre = user.Nombre,
                Direccion = user.Direccion,
                Telefono = user.Telefono,
                Deuda = user.Deuda
            };

            return Ok(userDto);
        }

        [HttpGet("ObtenerComprasUsuario/{userId}")]
        [SwaggerOperation("Obtiene las compras de un usuario")]
        public IActionResult ObtenerComprasUsuario(int userId)
        {
            try
            {
                var compras = _context.Compras.Where(c => c.UserId == userId).ToList();
                return Ok(compras);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las compras del usuario: {ex.Message}");
            }
        }



    }
}
