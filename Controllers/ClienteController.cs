using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace K_F_ClothingStore.Controllers
{
    public class ClienteController : Controller
    {
        private readonly AccesoDatos _acceso;

        public ClienteController(AccesoDatos acceso)
        {
            _acceso = acceso;
        }

        public IActionResult Perfil()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
                return RedirectToAction("Index", "Home");

            RegistroViewModel modelo = _acceso.ObtenerPerfilUsuario((int)idUsuario);
            return View(modelo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuardarPerfil(RegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Formulario inválido. Revisa los datos ingresados.";
                return View("PerfilUsuario", model);
            }

            bool actualizado = _acceso.ActualizarPerfilUsuario(model);

            if (actualizado)
            {
                TempData["mensaje"] = "Perfil actualizado correctamente.";
            }
            else
            {
                TempData["error"] = "Error al actualizar el perfil.";
            }

            return RedirectToAction("Perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarCuenta()
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                TempData["error"] = "No se encontró el usuario en sesión.";
                return RedirectToAction("Login", "Auth");
            }

            bool eliminado = _acceso.EliminarPerfilUsuario(idUsuario.Value);

            if (eliminado)
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = "No se pudo eliminar el perfil.";
                return RedirectToAction("Perfil");
            }
        }

    }
}