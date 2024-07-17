namespace DemoPilotoV1.DTOS
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public IFormFile ImagenData { get; set; } // Archivo de imagen

        public string ImageFileName { get; set; }

        public string CodigoQR { get; set; } // Nueva propiedad

    }
}
