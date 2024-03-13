using static System.Net.Mime.MediaTypeNames;

namespace DemoPilotoV1.Clases
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public byte[] ImageData { get; set; }

        // Constructor
        public Product()
        {
            // Inicializa el arreglo de bytes en el constructor si es necesario
            ImageData = Array.Empty<byte>();
        }

    }

}
