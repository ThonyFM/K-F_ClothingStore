namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Persona {
        public Persona() {}

        public Persona(int id, string nombre1, string nombre2, string apellido1, string apellido2, string documentoIdentidad, string telefono, string email, DateTime? fechaNacimiento, string genero, int direccionID, string creadoPor)
        {
            ID = id;
            Nombre1 = nombre1;
            Nombre2 = nombre2;
            Apellido1 = apellido1;
            Apellido2 = apellido2;
            DocumentoIdentidad = documentoIdentidad;
            Telefono = telefono;
            Email = email;
            FechaNacimiento = fechaNacimiento;
            Genero = genero;
            DireccionID = direccionID;
            CreadoPor = creadoPor;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Nombre1 { get; set; }

        [StringLength(maximumLength: 50)]
        public string Nombre2 { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Apellido1 { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Apellido2 { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string DocumentoIdentidad { get; set; }

        [StringLength(maximumLength: 15)]
        public string Telefono { get; set; }

        [EmailAddress]
        [StringLength(maximumLength: 255)]
        public string Email { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(maximumLength: 50)]
        public string Genero { get; set; }
        

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(maximumLength: 50)]
        public string CreadoPor { get; set; }

        public DateTime? FechaModificacion { get; set; }

        [StringLength(maximumLength: 50)]
        public string ModificadoPor { get; set; }
        
        public int DireccionID { get; set; }

        public int? UsuarioID { get; set; }
    }
}
