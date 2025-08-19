using System.Linq;
using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace K_F_ClothingStore.Controllers;

[Route("Cliente")]
public class ClienteController : Controller
{
    private readonly AccesoDatos _accesoDatos;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(AccesoDatos accesoDatos, ILogger<ClienteController> logger)
    {
        _accesoDatos = accesoDatos;
        _logger = logger;
    }

    [HttpGet("Perfil")]
    public IActionResult Perfil()
    {
        try
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                _logger.LogWarning("Intento de acceso a perfil sin sesión");
                return RedirectToAction("InicioSesion", "Auth");
            }

            var modelo = _accesoDatos.ObtenerPerfilUsuario(idUsuario.Value);
            if (modelo == null || modelo.Usuario?.ID == 0)
            {
                _logger.LogError("No se encontró perfil para el usuario {Id}", idUsuario);
                TempData["error"] = "No se encontró tu perfil.";
                return RedirectToAction("InicioSesion", "Auth");
            }

            modelo.Usuario   ??= new Usuario();
            modelo.Persona   ??= new Persona();
            modelo.Direccion ??= new Direccion();
            modelo.Cliente   ??= new Cliente();

            return View(modelo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cargar perfil");
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost("GuardarPerfil")]
    [ValidateAntiForgeryToken]
    public IActionResult GuardarPerfil([FromForm] RegistroViewModel model)
    {
        try
        {
            var idSesion = HttpContext.Session.GetInt32("IdUsuario");
            if (idSesion == null || model.Usuario?.ID != idSesion.Value)
            {
                _logger.LogWarning("Intento de actualización con sesión inválida (Sesion={Sesion}, Modelo={Modelo})",
                    idSesion, model.Usuario?.ID);
                return Unauthorized();
            }

            // Quitar del ModelState campos que NO envías en el modal
            var keysAEliminar = new[]
            {
                // Usuario
                "Usuario.ContrasenaHash",
                "Usuario.Estado",
                "Usuario.FechaCreacion",
                "Usuario.FechaModificacion",
                // Persona
                "Persona.DocumentoIdentidad",
                "Persona.Email",
                "Persona.CreadoPor",
                "Persona.FechaCreacion",
                "Persona.FechaModificacion",
                "Persona.ModificadoPor",
                // Si Apellido2 no es obligatorio para ti:
                "Persona.Apellido2",
                // Direccion
                "Direccion.CreadoPor",
                "Direccion.FechaCreacion",
                "Direccion.FechaModificacion",
                "Direccion.ModificadoPor",
                // Cliente (no lo editas acá)
                "Cliente.PersonaID",
                "Cliente.CodigoCliente",
                "Cliente.Estado",
                "Cliente.CreadoPor",
                "Cliente.FechaCreacion",
                "Cliente.FechaModificacion",
                "Cliente.ModificadoPor"
            };
            foreach (var k in keysAEliminar)
                if (ModelState.ContainsKey(k)) ModelState.Remove(k);

            if (!ModelState.IsValid)
            {
                var errores = string.Join(" | ",
                    ModelState.Where(kv => kv.Value.Errors.Count > 0)
                              .Select(kv => $"{kv.Key}: {string.Join(",", kv.Value.Errors.Select(e => e.ErrorMessage))}"));

                _logger.LogWarning("Modelo inválido en GuardarPerfil => {Errores}", errores);
                TempData["error"] = "Por favor, revisa los campos requeridos.";
                return RedirectToAction("Perfil");
            }

            var ok = _accesoDatos.ActualizarPerfilUsuario(model);
            if (!ok)
            {
                _logger.LogError("No se pudo actualizar el perfil en la base de datos");
                TempData["error"] = "No se pudo actualizar el perfil.";
                return RedirectToAction("Perfil");
            }

            TempData["mensaje"] = "Perfil actualizado correctamente.";
            _logger.LogInformation("Perfil actualizado para usuario {Id}", idSesion);
            return RedirectToAction("Perfil");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar perfil");
            TempData["error"] = "Ocurrió un error al guardar.";
            return RedirectToAction("Perfil");
        }
    }

    [HttpPost("EliminarCuenta")]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarCuenta(int idUsuario)
    {
        try
        {
            var sesionId = HttpContext.Session.GetInt32("IdUsuario");
            if (sesionId == null || sesionId.Value != idUsuario)
            {
                _logger.LogWarning("Intento de eliminación con sesión inválida (Sesion={Sesion}, Param={Param})",
                    sesionId, idUsuario);
                return Unauthorized();
            }

            var eliminado = _accesoDatos.EliminarPerfilUsuario(idUsuario);
            if (!eliminado)
            {
                _logger.LogError("No se pudo eliminar el perfil en la base de datos");
                TempData["error"] = "No se pudo eliminar la cuenta.";
                return RedirectToAction("Perfil");
            }

            HttpContext.Session.Clear();
            TempData["mensaje"] = "Tu cuenta fue eliminada correctamente.";
            _logger.LogInformation("Cuenta eliminada para usuario {Id}", idUsuario);
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cuenta");
            TempData["error"] = "Ocurrió un error al eliminar la cuenta.";
            return RedirectToAction("Perfil");
        }
    }
}
