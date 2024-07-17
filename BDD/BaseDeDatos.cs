﻿using DemoPilotoV1.Clases;
using Microsoft.EntityFrameworkCore;

namespace DemoPilotoV1.BDD
{
    public class BaseDeDatos : DbContext
    {
        public DbSet<Product> ProductsDos { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Compras> Compras { get; set; }
        public DbSet<ProveedorProducto> ProveedorProductos { get; set; }

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
        }
    }
}
