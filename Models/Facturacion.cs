namespace K_F_ClothingStore.Models;

public class Facturacion
{
    public Cliente Cliente { get; set; }
    public Factura Factura { get; set; }
    public Producto Producto { get; set; }
    public List<DetalleFactura> DetalleFactura { get; set; } = new();
    public List<CarritoItem> ProductosEnCarrito { get; set; } = new();
    public int FacturaID { get; set; }
    public decimal Total { get; set; }
    public decimal Subtotal { get; set; }
}