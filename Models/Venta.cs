using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Venta
    {
        [Key]
        public int VentaID { get; set; }

        [Required]
        public int EmpleadoID { get; set; }

        [Required]
        public int ClienteID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal Total { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public Venta() { }

        public Venta(int ventaID, int empleadoID, int clienteID, int productoID, int cantidad, decimal total)
        {
            VentaID = ventaID;
            EmpleadoID = empleadoID;
            ClienteID = clienteID;
            ProductoID = productoID;
            Cantidad = cantidad;
            Total = total;
        }
    }
}