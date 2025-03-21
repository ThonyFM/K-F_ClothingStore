namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Usuario {
        public Usuario()
        {

        }
        public Usuario(int id, string nombreUsuario, string contrasenaHash, string email, string rol, string estado, DateTime? fechaModificacion)
        {
            ID = id;
            NombreUsuario = nombreUsuario;
            ContrasenaHash = contrasenaHash;
            Email = email;
            Rol = rol;
            Estado = estado;
            FechaModificacion = fechaModificacion;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(maximumLength: 255)]
        public string ContrasenaHash { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(maximumLength: 255)]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Rol { get; set; } = "Cliente"; // Por defecto, el rol será "Cliente"

        [Required]
        [StringLength(maximumLength: 50)]
        public string Estado { get; set; } = "Activo"; // Por defecto, el estado será "Activo"

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }
    }
}
