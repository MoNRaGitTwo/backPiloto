using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoPilotoV1.Repositorios
{
    public class RepoProductos
    {
        private readonly BaseDeDatos _baseDeDatos;
        private readonly string _connectionString;

        public RepoProductos(BaseDeDatos baseDeDatos)
        {
            _baseDeDatos = baseDeDatos;
            _connectionString = _baseDeDatos.Database.GetDbConnection().ConnectionString; // Obtener la cadena de conexión desde el DbContext
        }

        public List<Product> ObtenerTodosLosProductos()
        {
            try
            {
                return _baseDeDatos.ProductsDos
                    .Select(p => new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Stock = p.Stock,
                        ImageFileName = p.ImageFileName,
                        CodigoQR = p.CodigoQR,
                        ImageData = p.ImageData // Incluir los datos de la imagen directamente
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
            return _baseDeDatos.ProductsDos
                .FirstOrDefault(p => p.Id == id);
        }

        public void GuardarProducto(string name, decimal price, int stock, byte[] imageData, string imageFileName, string codigoQR)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                var query = "INSERT INTO ProductsDos (Name, Price, Stock, ImageData, ImageFileName, CodigoQR) VALUES (@name, @price, @stock, @imageData, @imageFileName, @codigoQR)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@stock", stock);
                    command.Parameters.AddWithValue("@imageData", imageData ?? (object)DBNull.Value); // Manejo de datos nulos
                    command.Parameters.AddWithValue("@imageFileName", imageFileName);
                    command.Parameters.AddWithValue("@codigoQR", codigoQR);

                    command.ExecuteNonQuery();
                }
            }
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
            var producto = _baseDeDatos.ProductsDos
                .FirstOrDefault(p => p.Id == id);

            if (producto != null)
            {
                producto.Name = name;
                producto.Price = price;
                producto.Stock = stock;

                // Solo actualizar la imagen si se proporciona una nueva
                if (imageData != null && imageData.Length > 0)
                {
                    producto.ImageFileName = imageFileName;
                    producto.ImageData = imageData; // Actualizar los datos de la imagen
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
