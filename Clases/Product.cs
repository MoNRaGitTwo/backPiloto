﻿using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;

namespace DemoPilotoV1.Clases
{
    [Table("ProductsDos")] // Asegúrate de usar el nombre correcto de la tabla
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageFileName { get; set; }
        public byte[] ImageData { get; set; } // Datos de la imagen como LONGBLOB
        public string CodigoQR { get; set; }
        public ICollection<ProveedorProducto> ProveedorProductos { get; set; } = new List<ProveedorProducto>();
    }

}
