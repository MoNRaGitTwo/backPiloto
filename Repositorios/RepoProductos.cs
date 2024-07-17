using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DemoPilotoV1.Repositorios
{
    public class RepoProductos
    {
        private readonly BaseDeDatos _baseDeDatos;

        public RepoProductos(BaseDeDatos baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
        }

        public List<Product> ObtenerTodosLosProductos()
        {
            try
            {
                return _baseDeDatos.ProductsDos.Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    ImageData = p.ImageData,
                    ImageFileName = p.ImageFileName,
                    CodigoQR = p.CodigoQR,
                 
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error al obtener todos los productos: {ex.Message}\n{ex.StackTrace}");
                throw;
            }
        }

        public Product ObtenerProductoPorId(int id)
        {
            return _baseDeDatos.ProductsDos.FirstOrDefault(p => p.Id == id);
        }

        public void GuardarProducto(string name, decimal price, int stock, byte[] imageData, string imageFileName, string codigoQR)
        {
            var producto = new Product
            {
                Name = name,
                Price = price,
                Stock = stock,
                ImageData = imageData,  // Asignamos los datos de la imagen al objeto Product
                ImageFileName = imageFileName,
                CodigoQR = codigoQR
            };

            // Guardamos el producto en la base de datos
            _baseDeDatos.ProductsDos.Add(producto);
            _baseDeDatos.SaveChanges();
        }




        public void EliminarProducto(int id)
        {
            var producto = ObtenerProductoPorId(id);
            if (producto != null)
            {
                _baseDeDatos.ProductsDos.Remove(producto);
                _baseDeDatos.SaveChanges();
            }
        }

        public Product EditarProducto(int id, string name, decimal price, int stock, byte[] imageData, string imageFileName)
        {
            var producto = _baseDeDatos.ProductsDos.Find(id);
            if (producto != null)
            {
                producto.Name = name;
                producto.Price = price;
                producto.Stock = stock;

                // Solo actualizar la imagen si se proporciona una nueva
                if (imageData != null && imageData.Length > 0)
                {
                    producto.ImageData = imageData;
                }

                if (!string.IsNullOrEmpty(imageFileName))
                {
                    producto.ImageFileName = imageFileName;
                }

                _baseDeDatos.SaveChanges();

                return producto; // Devolver el producto editado
            }

            return null;
        }

        public void ActualizarStock(int id, int nuevoStock)
        {
            var producto = ObtenerProductoPorId(id);
            if (producto != null)
            {
                producto.Stock = nuevoStock;
                _baseDeDatos.SaveChanges();
            }
        }

        public async Task AsociarProductoAProveedor(int proveedorId, int productoId)
        {
            var proveedorProducto = new ProveedorProducto
            {
                ProveedorId = proveedorId,
                ProductoId = productoId
            };

            _baseDeDatos.ProveedorProductos.Add(proveedorProducto);
            await _baseDeDatos.SaveChangesAsync();
        }

        public async Task<List<Product>> ObtenerProductosPorProveedor(int proveedorId)
        {
            return await _baseDeDatos.Proveedores
                .Where(p => p.Id == proveedorId)
                .SelectMany(p => p.ProveedorProductos)
                .Select(pp => pp.Producto)
                .ToListAsync();
        }
    }
}






