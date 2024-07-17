using System.ComponentModel.DataAnnotations.Schema;

namespace DemoPilotoV1.Clases
{

    [Table("users")] // Asegúrate de que el nombre de la tabla sea en minúsculas
    public class Users
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Password { get; set; }

    }
}
