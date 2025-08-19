using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
// <- para AddPicture(FileInfo)
using Border = iText.Layout.Borders.Border;
using Color = System.Drawing.Color;
using Path = System.IO.Path;

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
        if (!HttpContext.Session.TryGetValue("ClienteID", out _))
            return RedirectToAction("Login", "Cuenta");

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
            if (!HttpContext.Session.TryGetValue("ClienteID", out _))
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
        catch (Exception)
        {
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
            DetalleFactura = detalles,
            ProductosEnCarrito = detalles.Select(d => new CarritoItem
            {
                ProductoID = d.ProductoID,
                Precio = d.PrecioUnitario,
                Cantidad = d.Cantidad
            }).ToList(),
            Total = factura.Total
        };

        return View(model);
    }

    public IActionResult DescargarFactura(string facturaId)
    {
        // ✅ usar el parámetro si viene, si no, la sesión
        var idStr = !string.IsNullOrWhiteSpace(facturaId)
            ? facturaId
            : HttpContext.Session.GetString("FacturaID");

        if (string.IsNullOrEmpty(idStr)) return BadRequest("FacturaId requerido.");

        var factura = _acceso.ObtenerFacturaPorId(Convert.ToInt32(idStr));
        facturaId = HttpContext.Session.GetString("FacturaID");

        if (factura == null) return NotFound("Factura no encontrada");

        var detalles = _acceso.ObtenerDetallesFacturaPorFacturaID(Convert.ToInt32(facturaId));
        if (detalles == null || !detalles.Any()) return NotFound("Detalles de la factura no encontrados");

        var cliente = _acceso.ObtenerClientePorId(factura.ClienteID);
        if (cliente == null) return NotFound("Cliente no encontrado");

        // Colores corporativos
        var brand = new DeviceRgb(0x17, 0x39, 0x4A); // #17394a
        var accent = new DeviceRgb(0xB8, 0xA2, 0x69); // dorado suave
        var zebra = new DeviceRgb(0xF6, 0xF8, 0xFA);

        using var ms = new MemoryStream();
        using var writer = new PdfWriter(ms);
        using var pdf = new PdfDocument(writer);
        var doc = new Document(pdf, PageSize.A4);
        doc.SetMargins(36, 28, 40, 28);

        var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        var regular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

        // ---------- Encabezado con logo + barra de color ----------
        var header = new Table(UnitValue.CreatePercentArray(new float[] { 18, 82 }))
            .UseAllAvailableWidth()
            .SetBorder(Border.NO_BORDER);

        var logoCell = new Cell().SetBorder(Border.NO_BORDER).SetBackgroundColor(brand).SetPadding(10);
        var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Img", "logo_kf_clothingstore.png");
        if (System.IO.File.Exists(logoPath))
            try
            {
                var img = new Image(ImageDataFactory.Create(logoPath)).SetWidth(90);
                logoCell.Add(img);
            }
            catch
            {
                logoCell.Add(new Paragraph("K&F").SetFont(bold).SetFontSize(18).SetFontColor(ColorConstants.WHITE));
            }
        else
            logoCell.Add(new Paragraph("K&F").SetFont(bold).SetFontSize(18).SetFontColor(ColorConstants.WHITE));

        header.AddCell(logoCell);

        var rightHead = new Cell().SetBorder(Border.NO_BORDER).SetBackgroundColor(brand).SetPadding(12)
            .Add(new Paragraph("K&F CLOTHINGSTORE").SetFont(bold).SetFontSize(16).SetFontColor(ColorConstants.WHITE))
            .Add(new Paragraph($"Factura #{factura.ID}").SetFont(regular).SetFontSize(11)
                .SetFontColor(ColorConstants.WHITE));
        header.AddCell(rightHead);

        doc.Add(header);
        doc.Add(new LineSeparator(new SolidLine()).SetMarginTop(8).SetMarginBottom(12));

        // ---------- “Cards” de datos ----------
        var info = new Table(UnitValue.CreatePercentArray(new float[] { 50, 50 }))
            .UseAllAvailableWidth()
            .SetBorder(Border.NO_BORDER)
            .SetMarginBottom(10);

        var facturaCard = new Cell().SetBorder(new SolidBorder(brand, 0.75f))
            .SetPadding(10)
            .Add(new Paragraph("Datos de la factura").SetFont(bold).SetFontSize(12).SetFontColor(brand))
            .Add(new Paragraph($"Fecha: {factura.FechaCreacion:dd/MM/yyyy HH:mm}").SetFont(regular))
            .Add(new Paragraph($"Método de pago: {factura.MetodoPago}").SetFont(regular))
            .Add(new Paragraph($"Estado: {factura.Estado}").SetFont(regular));
        info.AddCell(facturaCard);

        var clienteCard = new Cell().SetBorder(new SolidBorder(brand, 0.75f))
            .SetPadding(10)
            .Add(new Paragraph("Datos del cliente").SetFont(bold).SetFontSize(12).SetFontColor(brand))
            .Add(new Paragraph($"Nombre: {HttpContext.Session.GetString("Nombre")}").SetFont(regular))
            .Add(new Paragraph($"Correo: {HttpContext.Session.GetString("Email")}").SetFont(regular))
            .Add(new Paragraph($"Identificación: {HttpContext.Session.GetString("Identificacion")}").SetFont(regular))
            .Add(new Paragraph($"Teléfono: {HttpContext.Session.GetString("Telefono")}").SetFont(regular));
        info.AddCell(clienteCard);

        doc.Add(info);

        // ---------- Detalle de productos ----------
        var table = new Table(UnitValue.CreatePercentArray(new float[] { 46, 12, 21, 21 }))
            .UseAllAvailableWidth();

        // encabezados
        Cell Th(string t, TextAlignment align)
        {
            return new Cell()
                .SetBackgroundColor(brand)
                .SetPadding(6)
                .Add(new Paragraph(t).SetFont(bold).SetFontColor(ColorConstants.WHITE))
                .SetTextAlignment(align);
        }

        table.AddHeaderCell(Th("PRODUCTO", TextAlignment.LEFT));
        table.AddHeaderCell(Th("CANT.", TextAlignment.CENTER));
        table.AddHeaderCell(Th("PRECIO UNIT.", TextAlignment.RIGHT));
        table.AddHeaderCell(Th("SUBTOTAL", TextAlignment.RIGHT));

        var stripe = false;
        var subtotal = 0m;

        foreach (var d in detalles)
        {
            var nombre = _acceso.ObtenerNombreProductoPorID(d.ProductoID);
            var bg = stripe ? zebra : ColorConstants.WHITE;

            table.AddCell(new Cell().SetBackgroundColor(bg).SetPadding(6).Add(new Paragraph(nombre)));
            table.AddCell(new Cell().SetBackgroundColor(bg).SetPadding(6).SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(d.Cantidad.ToString())));
            table.AddCell(new Cell().SetBackgroundColor(bg).SetPadding(6).SetTextAlignment(TextAlignment.RIGHT)
                .Add(new Paragraph($"₡{d.PrecioUnitario:N2}")));
            table.AddCell(new Cell().SetBackgroundColor(bg).SetPadding(6).SetTextAlignment(TextAlignment.RIGHT)
                .Add(new Paragraph($"₡{d.Subtotal:N2}")));
            stripe = !stripe;

            subtotal += d.Subtotal;
        }

        doc.Add(table);

        // ---------- Resumen / Totales ----------
        var totals = new Table(UnitValue.CreatePercentArray(new float[] { 60, 40 }))
            .UseAllAvailableWidth()
            .SetBorder(Border.NO_BORDER)
            .SetMarginTop(10);

        totals.AddCell(new Cell().SetBorder(Border.NO_BORDER)); // vacío a la izquierda

        var totalsBox = new Cell().SetBorder(new SolidBorder(brand, 0.75f)).SetPadding(10)
            .Add(new Paragraph("Resumen").SetFont(bold).SetFontColor(brand).SetMarginBottom(6))
            .Add(new Paragraph($"Subtotal:  ₡{subtotal:N2}").SetFont(regular).SetTextAlignment(TextAlignment.RIGHT))
            .Add(new Paragraph($"TOTAL:     ₡{factura.Total:N2}")
                .SetFont(bold).SetFontSize(14)
                .SetFontColor(accent)
                .SetTextAlignment(TextAlignment.RIGHT));
        totals.AddCell(totalsBox);

        doc.Add(totals);

        // ---------- Footer ----------
        doc.Add(new Paragraph("\n"));
        doc.Add(new LineSeparator(new SolidLine(0.5f)).SetMarginBottom(5));
        doc.Add(new Paragraph("Factura electrónica generada automáticamente - No requiere firma ni sello")
            .SetFont(regular).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(ColorConstants.DARK_GRAY));
        doc.Add(new Paragraph("K_F_ClothingStore © Todos los derechos reservados")
            .SetFont(regular).SetFontSize(8).SetTextAlignment(TextAlignment.CENTER)
            .SetFontColor(ColorConstants.DARK_GRAY));

        doc.Close();
        return File(ms.ToArray(), "application/pdf", $"Factura_{facturaId}.pdf");
    }

    public IActionResult DescargarFacturaExcel(string facturaId)
    {
        var idStr = !string.IsNullOrWhiteSpace(facturaId)
            ? facturaId
            : HttpContext.Session.GetString("FacturaID");

        if (string.IsNullOrEmpty(idStr)) return BadRequest("FacturaId requerido.");

        var factura = _acceso.ObtenerFacturaPorId(Convert.ToInt32(idStr));
        facturaId = HttpContext.Session.GetString("FacturaID");

        if (factura == null) return NotFound("Factura no encontrada");

        var detalles = _acceso.ObtenerDetallesFacturaPorFacturaID(Convert.ToInt32(facturaId));
        if (detalles == null) return NotFound("Detalles de la factura no encontrados");

        var cliente = _acceso.ObtenerClientePorId(factura.ClienteID);
        if (cliente == null) return NotFound("Cliente no encontrado");

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using var package = new ExcelPackage();
        var ws = package.Workbook.Worksheets.Add("Factura");
        ws.View.ShowGridLines = false;
        ws.PrinterSettings.Orientation = eOrientation.Portrait;

        // Paleta
        var brand = Color.FromArgb(0x17, 0x39, 0x4A); // #17394a
        var accent = Color.FromArgb(0xB8, 0xA2, 0x69);
        var zebra = Color.FromArgb(0xF6, 0xF8, 0xFA);

        // Logo (opcional y sin cargar en memoria)
        try
        {
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Img", "logo_kf_clothingstore.png");
            if (System.IO.File.Exists(logoPath))
            {
                var pic = ws.Drawings.AddPicture("LogoKF", new FileInfo(logoPath));
                pic.SetPosition(0, 2, 0, 2); // fila 1, col 1 (base 0)
                pic.SetSize(120);
                ws.Row(1).Height = 65;
            }
        }
        catch
        {
            /* ignorar errores del logo */
        }

        // Encabezado
        ws.Cells["B1:E1"].Merge = true;
        ws.Cells["B1"].Value = "K&F CLOTHINGSTORE";
        ws.Cells["B1"].Style.Font.Bold = true;
        ws.Cells["B1"].Style.Font.Size = 18;
        ws.Cells["B1"].Style.Font.Color.SetColor(Color.White);
        ws.Cells["B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        ws.Cells["B1"].Style.Fill.BackgroundColor.SetColor(brand);
        ws.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        ws.Cells["B1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        ws.Cells["B2:E2"].Merge = true;
        ws.Cells["B2"].Value = $"Factura #{factura.ID}";
        ws.Cells["B2"].Style.Font.Bold = true;
        ws.Cells["B2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

        // Empresa
        ws.Cells["A4"].Value = "Tienda de ropa y accesorios";
        ws.Cells["A5"].Value = "San José, Costa Rica";
        ws.Cells["A6"].Value = "Tel: +506 2222-2222";
        ws.Cells["A7"].Value = "Email: info@kfclothing.com";

        // Factura / Cliente
        ws.Cells["A9"].Value = "Datos de la factura";
        ws.Cells["A9"].Style.Font.Bold = true;
        ws.Cells["A10"].Value = $"Fecha: {factura.FechaCreacion:dd/MM/yyyy HH:mm}";
        ws.Cells["A11"].Value = $"Método de pago: {factura.MetodoPago}";
        ws.Cells["A12"].Value = $"Estado: {factura.Estado}";

        ws.Cells["C9"].Value = "Datos del cliente";
        ws.Cells["C9"].Style.Font.Bold = true;
        ws.Cells["C10"].Value = $"Nombre: {HttpContext.Session.GetString("Nombre")}";
        ws.Cells["C11"].Value = $"Correo: {HttpContext.Session.GetString("Email")}";
        ws.Cells["C12"].Value = $"Identificación: {HttpContext.Session.GetString("Identificacion")}";
        ws.Cells["C13"].Value = $"Teléfono: {HttpContext.Session.GetString("Telefono")}";

        // Cabecera detalle
        ws.Cells["A15"].Value = "Producto";
        ws.Cells["B15"].Value = "Cantidad";
        ws.Cells["C15"].Value = "Precio Unitario";
        ws.Cells["D15"].Value = "Subtotal";

        using (var r = ws.Cells["A15:D15"])
        {
            r.Style.Font.Bold = true;
            r.Style.Font.Color.SetColor(Color.White);
            r.Style.Fill.PatternType = ExcelFillStyle.Solid;
            r.Style.Fill.BackgroundColor.SetColor(brand);
            r.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        // Detalle
        var row = 16;
        var subtotal = 0m;

        foreach (var d in detalles)
        {
            var nombre = _acceso.ObtenerNombreProductoPorID(d.ProductoID);

            ws.Cells[row, 1].Value = nombre;
            ws.Cells[row, 2].Value = d.Cantidad;
            ws.Cells[row, 3].Value = d.PrecioUnitario;
            ws.Cells[row, 4].Value = d.Subtotal;

            ws.Cells[row, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Cells[row, 3].Style.Numberformat.Format = "₡#,##0.00";
            ws.Cells[row, 4].Style.Numberformat.Format = "₡#,##0.00";

            if (row % 2 == 0)
            {
                using var zr = ws.Cells[$"A{row}:D{row}"];
                zr.Style.Fill.PatternType = ExcelFillStyle.Solid;
                zr.Style.Fill.BackgroundColor.SetColor(zebra);
            }

            subtotal += d.Subtotal;
            row++;
        }

        // Resumen/Total (card a la derecha)
        ws.Cells[$"C{row}"].Value = "Subtotal:";
        ws.Cells[$"C{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        ws.Cells[$"D{row}"].Value = subtotal;
        ws.Cells[$"D{row}"].Style.Numberformat.Format = "₡#,##0.00";


        row++;
        ws.Cells[$"C{row}"].Value = "TOTAL:";
        ws.Cells[$"C{row}"].Style.Font.Bold = true;
        ws.Cells[$"C{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        ws.Cells[$"D{row}"].Value = factura.Total;
        ws.Cells[$"D{row}"].Style.Font.Bold = true;
        ws.Cells[$"D{row}"].Style.Font.Color.SetColor(accent);
        ws.Cells[$"D{row}"].Style.Numberformat.Format = "₡#,##0.00";
        ws.Cells[$"D{row}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

        // Bordes finos de la tabla productos
        using (var grid = ws.Cells[$"A15:D{row}"])
        {
            grid.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            grid.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            grid.Style.Border.Right.Style = ExcelBorderStyle.Thin;
            grid.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        }

        // Pie + ajustes de impresión
        ws.Cells[$"A{row + 2}:D{row + 2}"].Merge = true;
        ws.Cells[$"A{row + 2}"].Value = $"Factura generada automáticamente el {DateTime.Now:dd/MM/yyyy HH:mm}";
        ws.Cells[$"A{row + 2}"].Style.Font.Size = 8;
        ws.Cells[$"A{row + 2}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        ws.Cells[$"A{row + 2}"].Style.Font.Color.SetColor(Color.Gray);

        ws.Cells[$"A{row + 3}:D{row + 3}"].Merge = true;
        ws.Cells[$"A{row + 3}"].Value = "K_F_ClothingStore © Todos los derechos reservados";
        ws.Cells[$"A{row + 3}"].Style.Font.Size = 8;
        ws.Cells[$"A{row + 3}"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        ws.Cells[$"A{row + 3}"].Style.Font.Color.SetColor(Color.Gray);

        // Columnas / Freeze panes
        ws.View.FreezePanes(16, 1);
        ws.Column(1).Width = 42; // Producto
        ws.Column(2).Width = 12; // Cantidad
        ws.Column(3).Width = 18; // Precio
        ws.Column(4).Width = 18; // Subtotal

        // Encabezado y pie para imprimir
        ws.HeaderFooter.OddHeader.RightAlignedText = $"Factura #{factura.ID}";
        ws.HeaderFooter.OddFooter.CenteredText = "Página &P de &N";
        ws.PrinterSettings.RepeatRows = new ExcelAddress("15:15"); // repetir encabezado
        ws.PrinterSettings.FitToWidth = 1;
        ws.PrinterSettings.PrintArea = ws.Cells[$"A1:D{row + 3}"];

        // Metadatos
        package.Workbook.Properties.Author = "K_F_ClothingStore";
        package.Workbook.Properties.Title = $"Factura {factura.ID}";

        var bytes = package.GetAsByteArray();
        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            $"Factura_{facturaId}.xlsx");
    }
}