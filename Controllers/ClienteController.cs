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
            {
                Console.WriteLine("⚠️ No se encontró usuario en sesión al cargar perfil.");
                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine($"✅ Cargando perfil para UsuarioID = {idUsuario}");

            RegistroViewModel modelo = _acceso.ObtenerPerfilUsuario((int)idUsuario);

            if (modelo == null)
            {
                Console.WriteLine("⚠️ No se encontró perfil asociado.");
                TempData["error"] = "No se encontró el perfil del usuario.";
                return RedirectToAction("Index", "Home");
            }

            return View(modelo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GuardarPerfil(RegistroViewModel model)
        {
            Console.WriteLine("🔵 Iniciando proceso de actualizar perfil...");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ ModelState inválido. Errores:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"    -> {error.ErrorMessage}");
                }

                TempData["error"] = "Hay errores en el formulario. Corrígelos e intenta nuevamente.";
                return View("Perfil", model);
            }

            try
            {
                Console.WriteLine($"🔎 Intentando actualizar perfil para UsuarioID = {model.Usuario.ID}");

                bool actualizado = _acceso.ActualizarPerfilUsuario(model);

                if (actualizado)
                {
                    Console.WriteLine("✅ Perfil actualizado correctamente.");
                    TempData["mensaje"] = "Perfil actualizado correctamente.";
                }
                else
                {
                    Console.WriteLine("❌ Falló la actualización de perfil. Verificando datos actuales...");
                    TempData["error"] = "No se pudo actualizar el perfil. Verifica tus datos.";

                    model = _acceso.ObtenerPerfilUsuario(model.Usuario.ID);
                    return View("Perfil", model);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🛑 Excepción en actualizar perfil: {ex}");
                TempData["error"] = $"Error inesperado al actualizar el perfil: {ex.Message}";
            }

            return RedirectToAction("Perfil");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarCuenta()
        {
            Console.WriteLine("🔵 Iniciando proceso de eliminar cuenta...");

            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");

            if (idUsuario == null)
            {
                Console.WriteLine("⚠️ No se encontró usuario en sesión al intentar eliminar cuenta.");
                TempData["error"] = "Sesión inválida. Inicie sesión.";
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                Console.WriteLine($"🔎 Intentando eliminar UsuarioID = {idUsuario}");

                bool eliminado = _acceso.EliminarPerfilUsuario(idUsuario.Value);

                if (eliminado)
                {
                    Console.WriteLine("✅ Cuenta eliminada correctamente. Cerrando sesión...");
                    HttpContext.Session.Clear();
                    TempData["mensaje"] = "Cuenta eliminada exitosamente.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine("❌ No se pudo eliminar la cuenta (puede tener dependencias).");
                    TempData["error"] = "No se pudo eliminar la cuenta. Verifica que no tengas órdenes activas.";
                    return RedirectToAction("Perfil");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🛑 Excepción en eliminar cuenta: {ex}");
                TempData["error"] = $"Error inesperado al eliminar la cuenta: {ex.Message}";
                return RedirectToAction("Perfil");
            }
        }
    }
}
