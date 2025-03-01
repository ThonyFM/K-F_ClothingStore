namespace K_F_ClothingStore.Models
{
    public class Factura
    {
        public int ID { get; set; }
        public int ClienteID { get; set; }
        public int EmpleadoID { get; set; }
        public DateTime FechaEmision { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public string Estado { get; set; }
        public int? DescuentoID { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public Cliente Cliente { get; set; }       // Relación con Cliente
        public Empleado Empleado { get; set; }     // Relación con Empleado
        public Descuento Descuento { get; set; }   // Relación con Descuento
    }
}