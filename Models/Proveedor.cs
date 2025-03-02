using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Proveedor
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreEmpresa { get; set; }

        [StringLength(100)]
        public string NombreContacto { get; set; }

        [StringLength(15)]
        public string Telefono { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        [Required]
        public int DireccionID { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [Required]
        [StringLength(50)]
        public string CreadoPor { get; set; }

        [StringLength(50)]
        public string ModificadoPor { get; set; }

        public Proveedor() { }

        public Proveedor(int id, string nombreEmpresa, string nombreContacto, string telefono, string email, int direccionID, string estado, string creadoPor)
        {
            ID = id;
            NombreEmpresa = nombreEmpresa;
            NombreContacto = nombreContacto;
            Telefono = telefono;
            Email = email;
            DireccionID = direccionID;
            Estado = estado;
            CreadoPor = creadoPor;
        }
    }
}