using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Direccion
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Ciudad { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(20)]
        public string CodigoPostal { get; set; }

        [Required]
        [StringLength(50)]
        public string Pais { get; set; }

        [StringLength(20)]
        public string TipoDireccion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string CreadoPor { get; set; }

        public DateTime? FechaModificacion { get; set; }

        [StringLength(50)]
        public string ModificadoPor { get; set; }

        public Direccion() { }

        public Direccion(int id, string ciudad, string estado, string codigoPostal, string pais, string tipoDireccion, string creadoPor)
        {
            ID = id;
            Ciudad = ciudad;
            Estado = estado;
            CodigoPostal = codigoPostal;
            Pais = pais;
            TipoDireccion = tipoDireccion;
            CreadoPor = creadoPor;
        }
    }
}