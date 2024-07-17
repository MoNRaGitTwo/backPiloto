namespace DemoPilotoV1.Clases
{
    public class ProveedorProducto
    {
        public int ProveedorId { get; set; }
        public Proveedores Proveedor { get; set; }
        public int ProductoId { get; set; }
        public Product Producto { get; set; }
    }
}
