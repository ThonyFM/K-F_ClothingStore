namespace K_F_ClothingStore.Models
{
    public class DetalleFactura
    {
        public int ID { get; set; }
        public int FacturaID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public Factura Factura { get; set; }   // Relación con Factura
        public Producto Producto { get; set; } // Relación con Producto
        public DetalleFactura()
        {



        }
    }
}