using Microsoft.AspNetCore.Mvc;
using K_F_ClothingStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font.Constants;
using iText.Kernel.Font;

namespace K_F_ClothingStore.Controllers
{
    public class FacturacionController : Controller
    {
        private readonly AccesoDatos _acceso;

        public FacturacionController(AccesoDatos acceso)
        {
            _acceso = acceso;
        }

        public IActionResult Index()
        {
            if (!HttpContext.Session.TryGetValue("ClienteID", out var clienteIdBytes))
            {
                return RedirectToAction("Login", "Cuenta"); // Redirige si no hay sesión activa
            }

            int clienteId = int.Parse(HttpContext.Session.GetString("ClienteID"));
            var productosEnCarrito = _acceso.ObtenerCarritoPorClienteID(clienteId);
            var total = productosEnCarrito.Sum(p => p.Precio * p.Cantidad);

            var model = new Facturacion
            {
                Cliente = _acceso.ObtenerClientePorId(clienteId),
                ProductosEnCarrito = productosEnCarrito,
                Total = total
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult GenerarFactura()
        {
            try
            {
                if (!HttpContext.Session.TryGetValue("ClienteID", out var clienteIdBytes))
                {
                    return RedirectToAction("Login", "Cuenta");
                }

                int clienteId = int.Parse(HttpContext.Session.GetString("ClienteID") ?? string.Empty);
                var productosEnCarrito = _acceso.ObtenerCarritoPorClienteID(clienteId);
                if (!productosEnCarrito.Any())
                {
                    return BadRequest("El carrito está vacío.");
                }

                var total = productosEnCarrito.Sum(p => p.Precio * p.Cantidad);
                var factura = new Factura
                {
                    ClienteID = clienteId,
                    FechaCreacion = DateTime.Now,
                    Total = total,
                    MetodoPago = "Tarjeta de Crédito",
                    Estado = "Pendiente",
                    CreadoPor = "Sistema"
                };

                factura.ID = _acceso.AgregarFactura(factura);
                HttpContext.Session.SetString("FacturaID", factura.ID.ToString());
                
                foreach (var producto in productosEnCarrito)
                {
                    var detalleFactura = new DetalleFactura
                    {
                        FacturaID = factura.ID,
                        ProductoID = producto.ProductoID,
                        Cantidad = producto.Cantidad,
                        PrecioUnitario = producto.Precio,
                        Subtotal = producto.Precio * producto.Cantidad
                    };
                    _acceso.AgregarDetalleFactura(detalleFactura);
                }

                _acceso.EliminarCarritoPorCliente(clienteId);
                return RedirectToAction("Confirmacion", new { facturaId = factura.ID });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return RedirectToAction("Error");
            }
        }

        public IActionResult Confirmacion(int facturaId)
        {
            var factura = _acceso.ObtenerFacturaPorId(facturaId);
            if (factura == null) return NotFound("Factura no encontrada");

            var detalles = _acceso.ObtenerDetallesFacturaPorFacturaID(facturaId);
            if (detalles == null || !detalles.Any())
            {
                return NotFound("Detalles de la factura no encontrados");
            }

            var cliente = _acceso.ObtenerClientePorId(factura.ClienteID);
            if (cliente == null) return NotFound("Cliente no encontrado");

            var model = new Facturacion
            {
                Factura = factura,
                Cliente = cliente,
                DetalleFactura = detalles, // Asigna los detalles de la factura
                ProductosEnCarrito = detalles
                    .Select(detalle => new CarritoItem
                    {
                        ProductoID = detalle.ProductoID,
                        Precio = detalle.PrecioUnitario,
                        Cantidad = detalle.Cantidad,
                    })
                    .ToList(),
                Total = factura.Total
            };

            return View(model);
        }
  public IActionResult DescargarFactura(string facturaId)
{
    facturaId = HttpContext.Session.GetString("FacturaID");
    var factura = _acceso.ObtenerFacturaPorId(Convert.ToInt32(facturaId));
    if (factura == null) return NotFound("Factura no encontrada");

    var detalles = _acceso.ObtenerDetallesFacturaPorFacturaID(Convert.ToInt32(facturaId));
    if (detalles == null) return NotFound("Detalles de la factura no encontrados");

    var cliente = _acceso.ObtenerClientePorId(factura.ClienteID);
    if (cliente == null) return NotFound("Cliente no encontrado");

    using (MemoryStream ms = new MemoryStream())
    {
        using (PdfWriter writer = new PdfWriter(ms))
        {
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                Document document = new Document(pdf);
                document.SetMargins(40, 40, 60, 40);

                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                // Título
                document.Add(new Paragraph("Factura Electrónica")
                    .SetFont(boldFont)
                    .SetFontSize(22)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLUE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20));

                // Información de Factura
                document.Add(new Paragraph($"Factura ID: {factura.ID}")
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Fecha de emisión: {factura.FechaCreacion:dd/MM/yyyy}")
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Método de Pago: {factura.MetodoPago}")
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Estado: {factura.Estado}")
                    .SetFont(regularFont));
                document.Add(new Paragraph("\n"));

                // Tabla de Detalles
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 4, 2, 2, 2 }))
                    .UseAllAvailableWidth()
                    .SetMarginBottom(20);

                table.AddHeaderCell(new Cell().Add(new Paragraph("Producto").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Cantidad").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Precio Unitario").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Subtotal").SetFont(boldFont)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));

                bool alternar = false;
                foreach (var detalle in detalles)
                {
                    var background = alternar ? iText.Kernel.Colors.ColorConstants.WHITE : iText.Kernel.Colors.ColorConstants.LIGHT_GRAY;
                    table.AddCell(new Cell().Add(new Paragraph($"Producto {detalle.ProductoID}")).SetBackgroundColor(background));
                    table.AddCell(new Cell().Add(new Paragraph(detalle.Cantidad.ToString())).SetBackgroundColor(background).SetTextAlignment(TextAlignment.CENTER));
                    table.AddCell(new Cell().Add(new Paragraph($"₡{detalle.PrecioUnitario:N2}")).SetBackgroundColor(background).SetTextAlignment(TextAlignment.RIGHT));
                    table.AddCell(new Cell().Add(new Paragraph($"₡{detalle.Subtotal:N2}")).SetBackgroundColor(background).SetTextAlignment(TextAlignment.RIGHT));
                    alternar = !alternar;
                }

                document.Add(table);

                // Total
                document.Add(new Paragraph($"Total a Pagar: ₡{factura.Total:N2}")
                    .SetFont(boldFont)
                    .SetFontSize(16)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.GREEN)
                    .SetTextAlignment(TextAlignment.RIGHT));

                // Pie de página
                document.Add(new Paragraph("\nGracias por su compra.\nFactura generada electrónicamente el " + DateTime.Now.ToString("dd/MM/yyyy"))
                    .SetFont(regularFont)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginTop(30));
            }
        }
        return File(ms.ToArray(), "application/pdf", $"Factura_{facturaId}.pdf");
    }
}

    }
}