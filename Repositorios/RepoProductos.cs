using DemoPilotoV1.BDD;
using DemoPilotoV1.Clases;

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
            return _baseDeDatos.Products.ToList();
        }
        public Product ObtenerProductoPorId(int id)
        {
            return _baseDeDatos.Products.FirstOrDefault(p => p.Id == id);
        }

        public void GuardarProducto(string nombre, decimal precio, byte[] imagenData)
        {
            var nuevoProducto = new Product
            {
                Name = nombre,
                Price = precio,
                ImageData = imagenData
                // Asigna otras propiedades según sea necesario
            };

            _baseDeDatos.Products.Add(nuevoProducto);
            _baseDeDatos.SaveChanges();
        }
    }
}
