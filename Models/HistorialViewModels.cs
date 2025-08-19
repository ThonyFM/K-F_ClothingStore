namespace K_F_ClothingStore.Models;

public class HistorialCompraVm
{
    public int FacturaID { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public string MetodoPago { get; set; }
    public string Estado { get; set; }
    public List<HistorialItemVm> Items { get; set; } = new();
}

public class HistorialItemVm
{
    public int ProductoID { get; set; }
    public string Nombre { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => PrecioUnitario * Cantidad;

    public string ImagenUrl { get; set; } // guarda solo el nombre (ej: VestidoFloral.jpg)
    public string Talla { get; set; }
    public string Color { get; set; }
}