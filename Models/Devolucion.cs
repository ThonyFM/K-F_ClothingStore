namespace K_F_ClothingStore.Models
{
    public class Devolucion
    {
        public int ID { get; set; }
        public int FacturaID { get; set; }
        public int DetalleFacturaID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public string Motivo { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public Factura Factura { get; set; }           // Relación con Factura
        public DetalleFactura DetalleFactura { get; set; } // Relación con DetalleFactura
        public Producto Producto { get; set; }         // Relación con Producto
    }
}