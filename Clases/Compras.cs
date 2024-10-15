namespace DemoPilotoV1.Clases
{
    public class Compras
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Fecha { get; set; }
        public string Detalle { get; set; }
        public decimal Total { get; set; }
    }
}
