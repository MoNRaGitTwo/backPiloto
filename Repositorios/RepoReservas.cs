using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DemoPilotoV1.Repositorios
{
    public class RepoReservas
    {
        private readonly BaseDeDatos _context;

        public RepoReservas(BaseDeDatos context)
        {
            _context = context;
        }

        public async Task<List<Reservas>> GetAllReservas()
        {
            return await _context.Reservas.ToListAsync();
        }

        public async Task<Reservas> GetReservaById(int id)
        {
            return await _context.Reservas.FindAsync(id);
        }

        public async Task CreateReserva(Reservas reserva)
        {
            await _context.Reservas.AddAsync(reserva);
            await _context.SaveChangesAsync();
        }
    }
}
