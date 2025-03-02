using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace K_F_ClothingStore.Controllers
{
    public class AuthController : Controller
    {
        private readonly AccesoDatos _acceso;

        public AuthController(AccesoDatos acceso)
        {
            _acceso = acceso;
        }
 
        // GET: Auth/Registro
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Auth/Registro
        [HttpPost]
        public IActionResult Registro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            try
            {
                // Asegúrate de que el rol sea "Cliente" por defecto
                usuario.Rol = "Cliente";
                usuario.Estado = "Activo";

                _acceso.AgregarUsuario(usuario);
                TempData["SuccessMessage"] = "Registro exitoso. Por favor, inicia sesión.";
                return RedirectToAction("InicioSesion");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al registrar el usuario: " + ex.Message;
                return View(usuario);
            }
        }

        // GET: Auth/InicioSesion
        public IActionResult InicioSesion()
        {
            return View();
        }

        // POST: Auth/InicioSesion
        [HttpPost]
        public IActionResult InicioSesion(string nombreUsuario, string contrasenaHash)
        {
            try
            {
                var usuario = _acceso.ObtenerUsuarioPorNombreUsuario(nombreUsuario);

                if (usuario == null || usuario.ContrasenaHash != contrasenaHash)
                {
                    TempData["ErrorMessage"] = "Nombre de usuario o contraseña incorrectos.";
                    return View();
                }

                // Guardar el usuario en la sesión
                HttpContext.Session.SetString("UsuarioID", usuario.ID.ToString());
                HttpContext.Session.SetString("Rol", usuario.Rol); // Guardar el rol del usuario

                TempData["SuccessMessage"] = "Inicio de sesión exitoso.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al iniciar sesión: " + ex.Message;
                return View();
            }
        }

        // GET: Auth/CerrarSesion
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Sesión cerrada correctamente.";
            return RedirectToAction("InicioSesion");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
        
    }
}
