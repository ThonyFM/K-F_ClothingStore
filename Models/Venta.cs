namespace K_F_ClothingStore.Models
{
    public class Venta
    {
        public int VentaID { get; set; }
        public int EmpleadoID { get; set; }
        public int ClienteID { get; set; }
        public int ProductoID { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public DateTime Fecha { get; set; }
        public Empleado Empleado { get; set; } // Relación con Empleado
        public Cliente Cliente { get; set; }   // Relación con Cliente
        public Producto Producto { get; set; } // Relación con Producto
    }
}