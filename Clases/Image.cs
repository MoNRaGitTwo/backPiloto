using System.ComponentModel.DataAnnotations.Schema;

namespace DemoPilotoV1.Clases
{
    [Table("Images")]
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] ImageData { get; set; } // Datos de la imagen como LONGBLOB
    }

}
