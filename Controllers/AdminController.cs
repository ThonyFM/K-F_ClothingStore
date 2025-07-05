using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
// Nada de esto esta funcionando  solo la parte de ver los productos es un error en la base de datos o en el modele
namespace K_F_ClothingStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly AccesoDatos _acceso;
        private readonly ILogger<AdminController> _logger;

        public AdminController(AccesoDatos acceso, ILogger<AdminController> logger)
        {
            _acceso = acceso;
            _logger = logger;
        }

        public IActionResult Productos()
        {
            var productos = _acceso.ObtenerTodosLosProductos();
            return View(productos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarProducto(Producto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _acceso.AgregarProducto(model);
                    TempData["SuccessMessage"] = "Producto agregado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al agregar producto. Verifique los datos ingresados.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar producto");
                TempData["ErrorMessage"] = "Error al agregar producto.";
            }
            return RedirectToAction("Productos");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarProducto(Producto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.FechaModificacion = DateTime.Now;
                    _acceso.ActualizarProducto(model);
                    TempData["SuccessMessage"] = "Producto actualizado correctamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error al actualizar producto. Verifique los datos.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar producto");
                TempData["ErrorMessage"] = "Error al actualizar producto.";
            }
            return RedirectToAction("Productos");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarProducto(int id)
        {
            try
            {
                _acceso.EliminarProducto(id);
                TempData["SuccessMessage"] = "Producto eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar producto");
                TempData["ErrorMessage"] = "Error al eliminar producto.";
            }
            return RedirectToAction("Productos");
        }
    }
}