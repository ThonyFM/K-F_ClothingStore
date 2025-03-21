namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class DetalleFactura {
        public DetalleFactura() {}

        public DetalleFactura(int id, int facturaID, int productoID, int cantidad, decimal precioUnitario, decimal subtotal)
        {
            ID = id;
            FacturaID = facturaID;
            ProductoID = productoID;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Subtotal = subtotal;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public int FacturaID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public decimal PrecioUnitario { get; set; }

        [Required]
        public decimal Subtotal { get; set; }
    }
}
