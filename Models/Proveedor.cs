namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Proveedor {
        public Proveedor() {}

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

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string NombreEmpresa { get; set; }

        [StringLength(maximumLength: 100)]
        public string NombreContacto { get; set; }

        [StringLength(maximumLength: 15)]
        public string Telefono { get; set; }

        [EmailAddress]
        [StringLength(maximumLength: 255)]
        public string Email { get; set; }

        [Required]
        public int DireccionID { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Estado { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string CreadoPor { get; set; }

        [StringLength(maximumLength: 50)]
        public string ModificadoPor { get; set; }
    }
}
