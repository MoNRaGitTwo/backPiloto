using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoPilotoV1.Clases
{
    [Table("Pedidos")]
    public class Pedidos
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Cambiado de ClienteId a UserId
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }

        public List<DetallePedidos> DetallesPedidos { get; set; }
    }
}
