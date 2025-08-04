using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Border = iText.Layout.Borders.Border;
using Color = System.Drawing.Color;

namespace K_F_ClothingStore.Controllers;

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
            return RedirectToAction("Login", "Cuenta"); // Redirige si no hay sesión activa

        var clienteId = int.Parse(HttpContext.Session.GetString("ClienteID"));
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
                return RedirectToAction("Login", "Cuenta");

            var clienteId = int.Parse(HttpContext.Session.GetString("ClienteID") ?? string.Empty);
            var productosEnCarrito = _acceso.ObtenerCarritoPorClienteID(clienteId);
            if (!productosEnCarrito.Any()) return BadRequest("El carrito está vacío.");

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
        if (detalles == null || !detalles.Any()) return NotFound("Detalles de la factura no encontrados");

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
                    Cantidad = detalle.Cantidad
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

        using (var ms = new MemoryStream())
        {
            using (var writer = new PdfWriter(ms))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf, PageSize.A4);
                    document.SetMargins(50, 35, 50, 35);

                    // Fuentes personalizadas
                    var boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                    var regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                    var titleFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                    // Encabezado con logo y datos de la empresa
                    var headerTable = new Table(UnitValue.CreatePercentArray(new float[] { 30, 70 }))
                        .UseAllAvailableWidth()
                        .SetBorder(Border.NO_BORDER);

                    // Aquí podrías añadir una imagen si tuvieras logo
                    // Cell logoCell = new Cell().Add(new Image(ImageDataFactory.Create("logo.png")).SetWidth(100));
                    var logoCell = new Cell().Add(new Paragraph("K_F_ClothingStore")
                        .SetFont(titleFont)
                        .SetFontSize(20)
                        .SetFontColor(new DeviceRgb(0, 51, 102)));
                    headerTable.AddCell(logoCell);

                    var companyCell = new Cell()
                        .SetTextAlignment(TextAlignment.RIGHT)
                        .Add(new Paragraph("K_F_ClothingStore")
                            .SetFont(boldFont)
                            .SetFontSize(12))
                        .Add(new Paragraph("Tienda de ropa y accesorios")
                            .SetFont(regularFont)
                            .SetFontSize(10))
                        .Add(new Paragraph("San José, Costa Rica")
                            .SetFont(regularFont)
                            .SetFontSize(10))
                        .Add(new Paragraph("Tel: +506 2222-2222")
                            .SetFont(regularFont)
                            .SetFontSize(10))
                        .Add(new Paragraph("Email: info@kfclothing.com")
                            .SetFont(regularFont)
                            .SetFontSize(10));

                    headerTable.AddCell(companyCell);
                    document.Add(headerTable);

                    // Línea divisoria
                    document.Add(new LineSeparator(new SolidLine())
                        .SetMarginTop(10)
                        .SetMarginBottom(15));

                    // Título del documento
                    document.Add(new Paragraph("FACTURA ELECTRÓNICA")
                        .SetFont(titleFont)
                        .SetFontSize(18)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(new DeviceRgb(0, 51, 102))
                        .SetMarginBottom(20));

                    // Información de factura y cliente en dos columnas
                    var infoTable = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 }))
                        .UseAllAvailableWidth()
                        .SetBorder(Border.NO_BORDER)
                        .SetMarginBottom(20);

                    // Columna izquierda - Datos de factura
                    var invoiceDataCell = new Cell()
                        .Add(new Paragraph("Datos de la Factura")
                            .SetFont(boldFont)
                            .SetFontSize(12)
                            .SetMarginBottom(5))
                        .Add(new Paragraph($"No. Factura: {factura.ID}")
                            .SetFont(regularFont))
                        .Add(new Paragraph($"Fecha: {factura.FechaCreacion:dd/MM/yyyy HH:mm}")
                            .SetFont(regularFont))
                        .Add(new Paragraph($"Método de Pago: {factura.MetodoPago}")
                            .SetFont(regularFont))
                        .Add(new Paragraph($"Estado: {factura.Estado}")
                            .SetFont(regularFont));

                    // Columna derecha - Datos del cliente
                    var clientDataCell = new Cell()
                        .Add(new Paragraph("Datos del Cliente")
                            .SetFont(boldFont)
                            .SetFontSize(12)
                            .SetMarginBottom(5))
                        .Add(new Paragraph($"Nombre: {HttpContext.Session.GetString("Nombre")}")
                            .SetFont(regularFont))
                        .Add(new Paragraph($"Correo: {HttpContext.Session.GetString("Email")}")
                            .SetFont(regularFont))
                        .Add(new Paragraph($"Identificación: {HttpContext.Session.GetString("Identificacion")}")
                            .SetFont(regularFont))
                        .Add(new Paragraph($"Teléfono: {HttpContext.Session.GetString("Telefono")}")
                            .SetFont(regularFont));

                    infoTable.AddCell(invoiceDataCell);
                    infoTable.AddCell(clientDataCell);
                    document.Add(infoTable);

                    // Tabla de detalles de productos
                    var productsTable = new Table(UnitValue.CreatePercentArray(new float[] { 45, 15, 20, 20 }))
                        .UseAllAvailableWidth()
                        .SetMarginTop(20)
                        .SetMarginBottom(30);

                    // Encabezados de la tabla
                    productsTable.AddHeaderCell(new Cell()
                        .SetBackgroundColor(new DeviceRgb(0, 51, 102))
                        .SetPadding(5)
                        .Add(new Paragraph("PRODUCTO")
                            .SetFont(boldFont)
                            .SetFontColor(ColorConstants.WHITE)));

                    productsTable.AddHeaderCell(new Cell()
                        .SetBackgroundColor(new DeviceRgb(0, 51, 102))
                        .SetPadding(5)
                        .Add(new Paragraph("CANTIDAD")
                            .SetFont(boldFont)
                            .SetFontColor(ColorConstants.WHITE))
                        .SetTextAlignment(TextAlignment.CENTER));

                    productsTable.AddHeaderCell(new Cell()
                        .SetBackgroundColor(new DeviceRgb(0, 51, 102))
                        .SetPadding(5)
                        .Add(new Paragraph("PRECIO UNIT.")
                            .SetFont(boldFont)
                            .SetFontColor(ColorConstants.WHITE))
                        .SetTextAlignment(TextAlignment.RIGHT));

                    productsTable.AddHeaderCell(new Cell()
                        .SetBackgroundColor(new DeviceRgb(0, 51, 102))
                        .SetPadding(5)
                        .Add(new Paragraph("SUBTOTAL")
                            .SetFont(boldFont)
                            .SetFontColor(ColorConstants.WHITE))
                        .SetTextAlignment(TextAlignment.RIGHT));

                    // Filas de productos
                    var alternar = false;
                    foreach (var detalle in detalles)
                    {
                        var nombreProducto = _acceso.ObtenerNombreProductoPorID(detalle.ProductoID);
                        var background = alternar ? new DeviceRgb(240, 240, 240) : ColorConstants.WHITE;

                        productsTable.AddCell(new Cell()
                            .SetPadding(5)
                            .SetBackgroundColor(background)
                            .Add(new Paragraph(nombreProducto)));

                        productsTable.AddCell(new Cell()
                            .SetPadding(5)
                            .SetBackgroundColor(background)
                            .Add(new Paragraph(detalle.Cantidad.ToString()))
                            .SetTextAlignment(TextAlignment.CENTER));

                        productsTable.AddCell(new Cell()
                            .SetPadding(5)
                            .SetBackgroundColor(background)
                            .Add(new Paragraph($"₡{detalle.PrecioUnitario:N2}"))
                            .SetTextAlignment(TextAlignment.RIGHT));

                        productsTable.AddCell(new Cell()
                            .SetPadding(5)
                            .SetBackgroundColor(background)
                            .Add(new Paragraph($"₡{detalle.Subtotal:N2}"))
                            .SetTextAlignment(TextAlignment.RIGHT));

                        alternar = !alternar;
                    }

                    document.Add(productsTable);

                    // Totales
                    var totalsTable = new Table(UnitValue.CreatePercentArray(new float[] { 70, 30 }))
                        .UseAllAvailableWidth()
                        .SetBorder(Border.NO_BORDER);

                    totalsTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .Add(new Paragraph(
                                "Gracias por su compra. Para consultas, contacte a servicio al cliente: info@kfclothing.com")
                            .SetFont(regularFont)
                            .SetFontSize(9)));

                    totalsTable.AddCell(new Cell()
                        .SetBorder(Border.NO_BORDER)
                        .SetBackgroundColor(new DeviceRgb(230, 230, 230))
                        .SetPadding(8)
                        .Add(new Paragraph("TOTAL A PAGAR")
                            .SetFont(boldFont)
                            .SetTextAlignment(TextAlignment.RIGHT))
                        .Add(new Paragraph($"₡{factura.Total:N2}")
                            .SetFont(boldFont)
                            .SetFontSize(16)
                            .SetFontColor(new DeviceRgb(0, 100, 0))
                            .SetTextAlignment(TextAlignment.RIGHT)));

                    document.Add(totalsTable);

                    // Pie de página
                    document.Add(new Paragraph("\n"));
                    document.Add(new LineSeparator(new SolidLine(0.5f))
                        .SetMarginBottom(5));

                    document.Add(new Paragraph("Factura electrónica generada automáticamente el " +
                                               DateTime.Now.ToString("dd/MM/yyyy HH:mm") +
                                               " - No requiere firma ni sello")
                        .SetFont(regularFont)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(ColorConstants.DARK_GRAY));

                    document.Add(new Paragraph("K_F_ClothingStore - © Todos los derechos reservados")
                        .SetFont(regularFont)
                        .SetFontSize(8)
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetFontColor(ColorConstants.DARK_GRAY));
                }
            }

            return File(ms.ToArray(), "application/pdf", $"Factura_{facturaId}.pdf");
        }
    }

    public IActionResult DescargarFacturaExcel(string facturaId)
    {
        facturaId = HttpContext.Session.GetString("FacturaID");
        var factura = _acceso.ObtenerFacturaPorId(Convert.ToInt32(facturaId));
        if (factura == null) return NotFound("Factura no encontrada");

        var detalles = _acceso.ObtenerDetallesFacturaPorFacturaID(Convert.ToInt32(facturaId));
        if (detalles == null) return NotFound("Detalles de la factura no encontrados");

        var cliente = _acceso.ObtenerClientePorId(factura.ClienteID);
        if (cliente == null) return NotFound("Cliente no encontrado");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Necesario para .NET Core

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Factura");
            worksheet.View.ShowGridLines = false;
            worksheet.PrinterSettings.Orientation = eOrientation.Landscape;

            // ============== ESTILOS PREDEFINIDOS ==============
            var titleStyle = worksheet.Cells["A1:D1"].Style;
            titleStyle.Font.Bold = true;
            titleStyle.Font.Color.SetColor(Color.White);
            titleStyle.Fill.PatternType = ExcelFillStyle.Solid;
            titleStyle.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 51, 102)); // Azul oscuro
            titleStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            titleStyle.VerticalAlignment = ExcelVerticalAlignment.Center;

            var headerStyle = worksheet.Cells["A14:D14"].Style;
            headerStyle.Font.Bold = true;
            headerStyle.Font.Color.SetColor(Color.White);
            headerStyle.Fill.PatternType = ExcelFillStyle.Solid;
            headerStyle.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 51, 102));
            headerStyle.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            var totalStyle = worksheet.Cells[$"D{18 + detalles.Count()}"].Style;
            totalStyle.Font.Bold = true;
            totalStyle.Font.Color.SetColor(Color.Green);
            totalStyle.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            // ============== ENCABEZADO ==============
            worksheet.Cells["A1:D1"].Merge = true;
            worksheet.Cells["A1"].Value = "K_F_ClothingStore";
            worksheet.Row(1).Height = 30;

            worksheet.Cells["A2:D2"].Merge = true;
            worksheet.Cells["A2"].Value = "FACTURA ELECTRÓNICA";
            worksheet.Cells["A2"].Style.Font.Bold = true;
            worksheet.Cells["A2"].Style.Font.Size = 16;
            worksheet.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Row(2).Height = 25;

            // ============== DATOS DE LA EMPRESA ==============
            worksheet.Cells["A4"].Value = "Tienda de ropa y accesorios";
            worksheet.Cells["A5"].Value = "San José, Costa Rica";
            worksheet.Cells["A6"].Value = "Tel: +506 2222-2222";
            worksheet.Cells["A7"].Value = "Email: info@kfclothing.com";

            // ============== DATOS DE FACTURA Y CLIENTE ==============
            worksheet.Cells["A9"].Value = "Datos de la Factura";
            worksheet.Cells["A9"].Style.Font.Bold = true;
            worksheet.Cells["A10"].Value = $"No. Factura: {factura.ID}";
            worksheet.Cells["A11"].Value = $"Fecha: {factura.FechaCreacion:dd/MM/yyyy HH:mm}";
            worksheet.Cells["A12"].Value = $"Método de Pago: {factura.MetodoPago}";
            worksheet.Cells["A13"].Value = $"Estado: {factura.Estado}";

            worksheet.Cells["C9"].Value = "Datos del Cliente";
            worksheet.Cells["C9"].Style.Font.Bold = true;
            worksheet.Cells["C10"].Value = $"Nombre: {HttpContext.Session.GetString("Nombre")}";
            worksheet.Cells["C11"].Value = $"Correo: {HttpContext.Session.GetString("Email")}";
            worksheet.Cells["C12"].Value = $"Identificación: {HttpContext.Session.GetString("Identificacion")}";
            worksheet.Cells["C13"].Value = $"Teléfono:{HttpContext.Session.GetString("Telefono")}";

            // ============== TABLA DE PRODUCTOS ==============
            worksheet.Cells["A14"].Value = "Producto";
            worksheet.Cells["B14"].Value = "Cantidad";
            worksheet.Cells["C14"].Value = "Precio Unitario";
            worksheet.Cells["D14"].Value = "Subtotal";

            var row = 15;
            foreach (var detalle in detalles)
            {
                var nombreProducto = _acceso.ObtenerNombreProductoPorID(detalle.ProductoID);

                worksheet.Cells[$"A{row}"].Value = nombreProducto;
                worksheet.Cells[$"B{row}"].Value = detalle.Cantidad;
                worksheet.Cells[$"C{row}"].Value = detalle.PrecioUnitario;
                worksheet.Cells[$"D{row}"].Value = detalle.Subtotal;

                // Formato de moneda
                worksheet.Cells[$"C{row}"].Style.Numberformat.Format = "₡#,##0.00";
                worksheet.Cells[$"D{row}"].Style.Numberformat.Format = "₡#,##0.00";
                worksheet.Cells[$"B{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                // Filas alternadas
                if (row % 2 == 1)
                {
                    worksheet.Cells[$"A{row}:D{row}"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[$"A{row}:D{row}"].Style.Fill.BackgroundColor
                        .SetColor(Color.FromArgb(240, 240, 240));
                }

                row++;
            }

            // ============== TOTALES ==============
            worksheet.Cells[$"C{row}"].Value = "Total a Pagar:";
            worksheet.Cells[$"C{row}"].Style.Font.Bold = true;
            worksheet.Cells[$"C{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

            worksheet.Cells[$"D{row}"].Value = factura.Total;
            worksheet.Cells[$"D{row}"].Style.Font.Bold = true;
            worksheet.Cells[$"D{row}"].Style.Font.Color.SetColor(Color.Green);
            worksheet.Cells[$"D{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            worksheet.Cells[$"D{row}"].Style.Numberformat.Format = "₡#,##0.00";

            // ============== PIE DE PÁGINA ==============
            worksheet.Cells[$"A{row + 2}:D{row + 2}"].Merge = true;
            worksheet.Cells[$"A{row + 2}"].Value = "Factura electrónica generada automáticamente el " +
                                                   DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            worksheet.Cells[$"A{row + 2}"].Style.Font.Size = 8;
            worksheet.Cells[$"A{row + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"A{row + 2}"].Style.Font.Color.SetColor(Color.Gray);

            worksheet.Cells[$"A{row + 3}:D{row + 3}"].Merge = true;
            worksheet.Cells[$"A{row + 3}"].Value = "K_F_ClothingStore - © Todos los derechos reservados";
            worksheet.Cells[$"A{row + 3}"].Style.Font.Size = 8;
            worksheet.Cells[$"A{row + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            worksheet.Cells[$"A{row + 3}"].Style.Font.Color.SetColor(Color.Gray);

            // Autoajustar columnas
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            // Bordes para la tabla de productos
            var productsTable = worksheet.Cells[$"A14:D{row}"];
            productsTable.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            productsTable.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            productsTable.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            productsTable.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            // Convertir a bytes y descargar
            var excelBytes = package.GetAsByteArray();
            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Factura_{facturaId}.xlsx");
        }
    }
}