namespace DemoPilotoV1.DTOS
{
    public class PedidoDTO
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public DateTime FechaPedido { get; set; }
        public decimal Total { get; set; }
        public string Estado { get; set; }
        public List<DetallePedidoDTO> DetallesPedidos { get; set; }
    }
}
