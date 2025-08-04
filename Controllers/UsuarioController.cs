using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace K_F_ClothingStore.Controllers;

public class UsuarioController : Controller
{
    private readonly AccesoDatos _acceso;

    public UsuarioController(AccesoDatos acceso)
    {
        _acceso = acceso;
    }

    public IActionResult Perfil()
    {
        try
        {
            var usuarioId = HttpContext.Session.GetInt32("IdUsuario");
            if (usuarioId == null)
            {
                TempData["error"] = "Debe iniciar sesión para acceder a su perfil.";
                return RedirectToAction("InicioSesion", "Auth");
            }

            var perfil = _acceso.ObtenerPerfilUsuario(usuarioId.Value);

            return View(perfil);
        }
        catch (Exception ex)
        {
            TempData["error"] = $"Error al cargar el perfil: {ex.Message}";
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult GuardarPerfil(RegistroViewModel perfil)
    {
        try
        {
            var actualizado = _acceso.ActualizarPerfilUsuario(perfil);

            if (actualizado)
                TempData["mensaje"] = "Perfil actualizado correctamente.";
            else
                TempData["error"] = "No se pudo actualizar el perfil.";

            return RedirectToAction("Perfil");
        }
        catch (Exception ex)
        {
            TempData["error"] = $"Error al actualizar el perfil: {ex.Message}";
            return RedirectToAction("Perfil");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarCuenta(int idUsuario)
    {
        try
        {
            var eliminado = _acceso.EliminarPerfilUsuario(idUsuario);

            if (eliminado)
            {
                HttpContext.Session.Clear();
                TempData["mensaje"] = "Cuenta eliminada correctamente.";
                return RedirectToAction("Index", "Home");
            }

            TempData["error"] = "No se pudo eliminar la cuenta.";
            return RedirectToAction("Perfil");
        }
        catch (Exception ex)
        {
            TempData["error"] = $"Error al eliminar la cuenta: {ex.Message}";
            return RedirectToAction("Perfil");
        }
    }
}