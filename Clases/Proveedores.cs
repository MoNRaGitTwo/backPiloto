using System.ComponentModel.DataAnnotations.Schema;

namespace DemoPilotoV1.Clases
{
    [Table("proveedores")]
    public class Proveedores
    {
        public int Id { get; set; }
        public string Empresa { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }


        public ICollection<ProveedorProducto> ProveedorProductos { get; set; } = new List<ProveedorProducto>();
    }
}
