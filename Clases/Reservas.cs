using System.ComponentModel.DataAnnotations;

namespace DemoPilotoV1.Clases
{
    public class Reservas
    {
        [Key]
        public int ReservaId { get; set; }

        [Required]
        [StringLength(100)]
        public string ClienteNombre { get; set; }

        [Required]
        [StringLength(15)]
        public string ClienteTelefono { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        [Required]
        [StringLength(50)]
        public string Barbero { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Por defecto a "Pendiente"
    }
}
