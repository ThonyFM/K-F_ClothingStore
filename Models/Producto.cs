using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_F_ClothingStore.Models
{
    public class Producto
    {
        [Key] 
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreProducto { get; set; }

        [Required]
        [StringLength(20)]
        public string Genero { get; set; }

        [Required]
        [StringLength(50)]
        public string SegmentoEdad { get; set; }

        [Required]
        [StringLength(50)]
        public string TipoProducto { get; set; }

        [Required]
        [StringLength(30)]
        public string Color { get; set; }

        [Required]
        [StringLength(20)]
        public string Talla { get; set; }

        [Required]
        public int UnidadesDisponibles { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public string ImagenUrl { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [NotMapped] // Esta propiedad no se mapea a la base de datos
        public IFormFile ImagenArchivo { get; set; }
    }
}