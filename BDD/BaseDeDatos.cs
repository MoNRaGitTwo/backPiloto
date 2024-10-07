using Microsoft.EntityFrameworkCore;
using DemoPilotoV1.Clases;

namespace DemoPilotoV1.BDD
{
    public class BaseDeDatos : DbContext
    {
        public DbSet<Product> ProductsDos { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Compras> Compras { get; set; }
        public DbSet<Pedidos> Pedidos { get; set; }
        public DbSet<DetallePedidos> DetallesPedidos { get; set; }
        public DbSet<ProveedorProducto> ProveedorProductos { get; set; }

        public DbSet<Reservas> Reservas { get; set; }

        public DbSet<AudioFile> AudioFiles { get; set; }

        public BaseDeDatos(DbContextOptions<BaseDeDatos> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProveedorProducto>()
                .HasKey(pp => new { pp.ProveedorId, pp.ProductoId });

            modelBuilder.Entity<ProveedorProducto>()
                .HasOne(pp => pp.Proveedor)
                .WithMany(p => p.ProveedorProductos)
                .HasForeignKey(pp => pp.ProveedorId);

            modelBuilder.Entity<ProveedorProducto>()
                .HasOne(pp => pp.Producto)
                .WithMany(p => p.ProveedorProductos)
                .HasForeignKey(pp => pp.ProductoId);

            modelBuilder.Entity<Pedidos>()
                .HasMany(p => p.DetallesPedidos)
                .WithOne(d => d.Pedido)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DetallePedidos>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId);

            // Configuración de la tabla ProductsDos para asegurar el tipo de datos
            modelBuilder.Entity<Product>()
                .Property(p => p.ImageData)
                .HasColumnType("LONGBLOB");

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Reservas>().ToTable("Reservas");
        }
    }
}
