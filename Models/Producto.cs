namespace K_F_ClothingStore.Models
{
    public class Producto
    {
        public int ID { get; set; }
        public string NombreProducto { get; set; }
        public string Genero { get; set; }
        public string SegmentoEdad { get; set; }
        public string TipoProducto { get; set; }
        public string Color { get; set; }
        public string Talla { get; set; }
        public int UnidadesDisponibles { get; set; }
        public decimal Precio { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}