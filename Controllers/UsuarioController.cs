using Microsoft.AspNetCore.Mvc;
using K_F_ClothingStore.Models;
using Microsoft.Extensions.Logging;

namespace K_F_ClothingStore.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AccesoDatos _acceso;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(AccesoDatos acceso, ILogger<UsuarioController> logger)
        {
            _acceso = acceso;
            _logger = logger;
        }

        public IActionResult User()
        {
            try
            {
                string ID = HttpContext.Session.GetString("ClienteID");

                if (string.IsNullOrEmpty(ID))
                {
                    TempData["ErrorMessage"] = "El USUARIO no existe.";
                }

                var usuario = _acceso.ObtenerUsuarioPorId(Convert.ToInt32(ID)); 
                if (usuario != null)
                {
                    return Json(new { success = true, usuario });
                }

                return Json(new { success = false, message = "Usuario no encontrado" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ObtenerUsuario.");
                return Json(new { success = false, message = "Error interno del servidor" });
            }
        }
    }
}
