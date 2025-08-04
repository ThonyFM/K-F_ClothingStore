using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models;

public class DetalleFactura
{
    public DetalleFactura()
    {
    }

    public DetalleFactura(int id, int facturaID, int productoID, int cantidad, decimal precioUnitario, decimal subtotal,
        string nombreProducto, string Descripcion, DateTime FechaCreacion)
    {
        ID = id;
        FacturaID = facturaID;
        string? nombreproducto;
        ProductoID = productoID;
        Cantidad = cantidad;
        PrecioUnitario = precioUnitario;
        Subtotal = subtotal;
    }

    [Key] public int ID { get; set; }

    [Required] public int FacturaID { get; set; }

    [Required] public int ProductoID { get; set; }

    public Producto Producto { get; set; }

    [Required] public int Cantidad { get; set; }

    [Required] public decimal PrecioUnitario { get; set; }

    [Required] public decimal Subtotal { get; set; }

    public string NombreProducto { get; set; }
    public DateTime FechaFactura { get; set; }
}