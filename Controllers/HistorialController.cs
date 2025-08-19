using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace K_F_ClothingStore.Controllers;

[Route("Historial")]
public class HistorialController : Controller
{
    private readonly AccesoDatos _acceso;
    private readonly ILogger<HistorialController> _logger;

    public HistorialController(AccesoDatos acceso, ILogger<HistorialController> logger)
    {
        _acceso = acceso;
        _logger = logger;
    }

    [HttpGet("")]
    public IActionResult Index(string? q, DateTime? desde, DateTime? hasta, string? estado)
    {
        if (!HttpContext.Session.TryGetValue("ClienteID", out _))
            return RedirectToAction("InicioSesion", "Auth");

        var clienteId = int.Parse(HttpContext.Session.GetString("ClienteID")!);

        // Detalles crudos por cliente
        var detalles =
            _acceso.ObtenerProductosCompradosPorCliente(
                clienteId); // List<DetalleFactura> (con FechaFactura, NombreProducto, etc.)

        // Agrupar por FacturaID
        var grupos = detalles
            .GroupBy(d => d.FacturaID)
            .Select(g =>
            {
                var fac = _acceso.ObtenerFacturaPorId(g.Key);
                var vm = new HistorialCompraVm
                {
                    FacturaID = g.Key,
                    Fecha = g.First().FechaFactura,
                    Total = g.Sum(x => x.Subtotal),
                    MetodoPago = fac?.MetodoPago ?? "-",
                    Estado = fac?.Estado ?? "-"
                };

                foreach (var d in g)
                {
                    // enriquecer con imagen/talla/color
                    var p = _acceso.ObtenerProductoPorId(d.ProductoID);
                    vm.Items.Add(new HistorialItemVm
                    {
                        ProductoID = d.ProductoID,
                        Nombre = d.NombreProducto ?? _acceso.ObtenerNombreProductoPorID(d.ProductoID),
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        ImagenUrl = p?.ImagenUrl, // recuerda: guardas solo el nombre (ej: VestidoFloral.jpg)
                        Talla = p?.Talla,
                        Color = p?.Color
                    });
                }

                return vm;
            })
            .ToList();

        // Filtros
        if (!string.IsNullOrWhiteSpace(q))
            grupos = grupos.Where(h =>
                    h.FacturaID.ToString().Contains(q.Trim(), StringComparison.OrdinalIgnoreCase) ||
                    h.Items.Any(i => (i.Nombre ?? "").Contains(q.Trim(), StringComparison.OrdinalIgnoreCase)))
                .ToList();

        if (desde.HasValue) grupos = grupos.Where(h => h.Fecha.Date >= desde.Value.Date).ToList();
        if (hasta.HasValue) grupos = grupos.Where(h => h.Fecha.Date <= hasta.Value.Date).ToList();
        if (!string.IsNullOrWhiteSpace(estado))
            grupos = grupos.Where(h => string.Equals(h.Estado, estado, StringComparison.OrdinalIgnoreCase)).ToList();

        // orden recientes primero
        grupos = grupos.OrderByDescending(h => h.Fecha).ToList();

        return View(grupos);
    }

    // Ver confirmación (detalles en tu view de Facturación)
    [HttpGet("Factura/{id:int}")]
    public IActionResult Factura(int id)
    {
        return RedirectToAction("Confirmacion", "Facturacion", new { facturaId = id });
    }

    // Descargas
    [HttpGet("DescargarPdf/{id:int}")]
    public IActionResult DescargarPdf(int id)
    {
        return RedirectToAction("DescargarFactura", "Facturacion", new { facturaId = id });
    }

    [HttpGet("DescargarExcel/{id:int}")]
    public IActionResult DescargarExcel(int id)
    {
        return RedirectToAction("DescargarFacturaExcel", "Facturacion", new { facturaId = id });
    }
}