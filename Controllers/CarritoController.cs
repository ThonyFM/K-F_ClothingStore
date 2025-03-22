using Microsoft.AspNetCore.Mvc;
using K_F_ClothingStore.Models;
using System;

namespace K_F_ClothingStore.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AccesoDatos _acceso;
        
       
        public IActionResult MiCarrito()
        {
            // Obtener el ID del cliente desde la sesión
            var clienteId = int.Parse(HttpContext.Session.GetString("ClienteID"));

            // Obtener los productos en el carrito del cliente
            var productosEnCarrito = _acceso.ObtenerCarritoPorClienteID(clienteId);

            return View(productosEnCarrito);
        }

        [HttpPost]
        public IActionResult ActualizarCantidad(int productoId, int cantidad)
        {
            try
            {
                var clienteId = int.Parse(HttpContext.Session.GetString("ClienteID"));

                // Actualizar la cantidad en el carrito
                _acceso.ActualizarCantidadCarrito(clienteId, productoId, cantidad);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult EliminarDelCarrito(int productoId)
        {
            try
            {
                var clienteId = int.Parse(HttpContext.Session.GetString("ClienteID"));

                // Eliminar el producto del carrito
                _acceso.EliminarProductoCarrito(clienteId, productoId);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public CarritoController(AccesoDatos acceso)
        {
            _acceso = acceso;
        }

        [HttpPost]
        public IActionResult AgregarAlCarrito(int productoId, int cantidad)
        {
            try
            {
                // Obtener el ID del cliente desde la sesión
                var clienteId = int.Parse(HttpContext.Session.GetString("ClienteID"));

                // Llamar al método de AccesoDatos para agregar al carrito
                _acceso.AgregarAlCarrito(clienteId, productoId, cantidad);

                return Json(new { success = true, message = "Producto agregado al carrito correctamente." });
                return View();
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
