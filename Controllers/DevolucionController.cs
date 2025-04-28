using Microsoft.AspNetCore.Mvc;
using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Http;

namespace K_F_ClothingStore.Controllers
{
    public class DevolucionController : Controller
    {
        private readonly AccesoDatos _acceso;

        public DevolucionController(AccesoDatos acceso)
        {
            _acceso = acceso;
        }

        // Muestra los productos comprados que pueden ser devueltos
        public IActionResult Index()
        {
            if (!HttpContext.Session.TryGetValue("ClienteID", out var clienteIdBytes))
            {
                return RedirectToAction("Login", "Cuenta"); // 🔒 Si no está logueado, va a login
            }

            int clienteID = int.Parse(HttpContext.Session.GetString("ClienteID"));

            var productosComprados = _acceso.ObtenerProductosCompradosPorCliente(clienteID);

            return View(productosComprados); // 🌟 Vista Solicitar.cshtml
        }

        // Procesa la solicitud de devolución
        [HttpPost]
        public IActionResult SolicitarDevolucion(int detalleFacturaID, int cantidad, string motivo)
        {
            if (!HttpContext.Session.TryGetValue("ClienteID", out var clienteIdBytes))
            {
                return RedirectToAction("Login", "Cuenta");
            }

            int clienteID = int.Parse(HttpContext.Session.GetString("ClienteID"));

            // 🔎 Primero obtengo el detalle de compra original
            var detalle = _acceso.ObtenerDetalleFacturaPorId(detalleFacturaID);

            if (detalle == null)
            {
                return NotFound("Detalle de factura no encontrado.");
            }

            // ⚡ Validaciones
            if (cantidad <= 0 || cantidad > detalle.Cantidad)
            {
                TempData["Error"] = "Cantidad inválida para devolución.";
                return RedirectToAction("Solicitar");
            }

            // 📅 Verificar si todavía está dentro del plazo de 7 días
            var factura = _acceso.ObtenerFacturaPorId(detalle.FacturaID);
            if (factura == null || (DateTime.Now - factura.FechaCreacion).TotalDays > 7)
            {
                TempData["Error"] = "El período para devolver este producto ha expirado.";
                return RedirectToAction("Solicitar");
            }

            // 🛠️ Crear nueva devolución
            var devolucion = new Devolucion
            {
                FacturaID = detalle.FacturaID,
                DetalleFacturaID = detalle.ID,
                ProductoID = detalle.ProductoID,
                Cantidad = cantidad,
                Motivo = motivo,
                Estado = "Pendiente",
                CreadoPor = "Cliente", // 🔹 O puedes personalizar con el nombre del usuario si quieres
            };

            bool resultado = _acceso.AgregarDevolucion(devolucion);

            if (resultado)
            {
                TempData["Exito"] = "Devolución solicitada correctamente.";
            }
            else
            {
                TempData["Error"] = "Error al solicitar la devolución.";
            }

            return RedirectToAction("Solicitar");
        }
    }
}
