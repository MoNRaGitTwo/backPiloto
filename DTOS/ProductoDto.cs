namespace DemoPilotoV1.DTOS
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImagenData { get; set; }
    }
}
