using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoPilotoV1.Clases
{
    [Table("Pedidos")]
    public class Pedidos
    {
         public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public List<DetallePedidos> DetallesPedidos { get; set; }
    }
}
