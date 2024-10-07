using DemoPilotoV1.Clases;
using DemoPilotoV1.BDD;
using Microsoft.EntityFrameworkCore;

namespace DemoPilotoV1.Repositorios
{
    public class UsersRepository
    {
        private readonly BaseDeDatos _context;

        public UsersRepository(BaseDeDatos context)
        {
            _context = context;
        }

        public async Task AddUserAsync(Users user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        // Nuevo método para actualizar la deuda de un usuario
        public async Task<bool> ActualizarDeudaAsync(int userId, decimal nuevaDeuda)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return false; // Si el usuario no se encuentra, devuelve false
            }

            user.Deuda = nuevaDeuda; // Actualiza la deuda del usuario
            await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

            return true; // Devuelve true si la actualización fue exitosa
        }

        public async Task<Users> GetUserByNameAsync(string nombre)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Nombre == nombre);
        }
    }
}
