using DemoPilotoV1.Clases;
using Microsoft.EntityFrameworkCore;

namespace DemoPilotoV1.BDD
{
    public class BaseDeDatos : DbContext
    {
        public DbSet<Product> Products { get; set; }
        



        public BaseDeDatos(DbContextOptions<BaseDeDatos> options)
            : base(options)
        {
        }
    }
}
