namespace K_F_ClothingStore.Models
{
    public class Usuario
    {
        public int ID { get; set; }
        public string NombreUsuario { get; set; }
        public string ContrasenaHash { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }


        public Usuario()
        {
            
        }

        public Usuario(int id, string nombreUsuario, string contrasenaHash, string email, string rol, string estado, DateTime fechaCreacion, DateTime? fechaModificacion)
        {
            ID = id;
            NombreUsuario = nombreUsuario;
            ContrasenaHash = contrasenaHash;
            Email = email;
            Rol = rol;
            Estado = estado;
            FechaCreacion = fechaCreacion;
            FechaModificacion = fechaModificacion;  
        }
    }
}
