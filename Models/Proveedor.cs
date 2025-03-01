namespace K_F_ClothingStore.Models
{
    public class Proveedor
    {
        public int ID { get; set; }
        public string NombreEmpresa { get; set; }
        public string NombreContacto { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public int DireccionID { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public Direccion Direccion { get; set; } // Relación con Direccion
    }
}