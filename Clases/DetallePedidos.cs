using System.ComponentModel.DataAnnotations.Schema;

namespace DemoPilotoV1.Clases
{
    [Table("DetallesPedidos")]
    public class DetallePedidos
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string Nombre { get; set; }  
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }       
        public decimal Precio { get; set; }
        public Pedidos Pedido { get; set; }
        public Product Producto { get; set; }


    }
}
