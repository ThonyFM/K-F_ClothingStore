using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Persona
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre1 { get; set; }

        [StringLength(50)]
        public string Nombre2 { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido1 { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido2 { get; set; }

        [Required]
        [StringLength(20)]
        public string DocumentoIdentidad { get; set; }

        [StringLength(15)]
        public string Telefono { get; set; }

        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(50)]
        public string Genero { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string CreadoPor { get; set; }

        public DateTime? FechaModificacion { get; set; }

        [StringLength(50)]
        public string ModificadoPor { get; set; }

        [Required]
        public int DireccionID { get; set; }

        public int? UsuarioID { get; set; }

        public Persona() { }

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
    }
}