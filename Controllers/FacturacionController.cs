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
            var factura = _acceso.ObtenerFacturaPorId(Convert.ToInt32( facturaId) );
            if (factura == null) return NotFound("Factura no encontrada");

            var detalles = _acceso.ObtenerDetallesFacturaPorFacturaID(Convert.ToInt32( facturaId));
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
                        PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                        document.Add(new Paragraph("Factura Electrónica")
                            .SetFont(boldFont)
                            .SetFontSize(20)
                            .SetTextAlignment(TextAlignment.CENTER));

                        document.Add(new Paragraph($"Factura ID: {factura.ID}"));
                        document.Add(new Paragraph($"Fecha de emisión: {factura.FechaCreacion:dd/MM/yyyy}"));
                        document.Add(new Paragraph($"Método de Pago: {factura.MetodoPago}"));
                        document.Add(new Paragraph($"Estado: {factura.Estado}"));
                        document.Add(new Paragraph("\n"));

                        Table table = new Table(4).UseAllAvailableWidth();
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Producto").SetFont(boldFont)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Cantidad").SetFont(boldFont)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Precio Unitario").SetFont(boldFont)));
                        table.AddHeaderCell(new Cell().Add(new Paragraph("Subtotal").SetFont(boldFont)));

                        foreach (var detalle in detalles)
                        {
                            table.AddCell(new Cell().Add(new Paragraph($"Producto {detalle.ProductoID}")));
                            table.AddCell(new Cell().Add(new Paragraph(detalle.Cantidad.ToString())));
                            table.AddCell(new Cell().Add(new Paragraph($"${detalle.PrecioUnitario:F2}")));
                            table.AddCell(new Cell().Add(new Paragraph($"${detalle.Subtotal:F2}")));
                        }

                        document.Add(table);
                        document.Add(new Paragraph($"Total a pagar: ${factura.Total:F2}")
                            .SetFont(boldFont)
                            .SetTextAlignment(TextAlignment.RIGHT));
                    }
                }
                return File(ms.ToArray(), "application/pdf", $"Factura_{facturaId}.pdf");
            }
        }
    }
}