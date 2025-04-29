using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;

[Route("Cliente")]
public class ClienteController : Controller
{
    private readonly AccesoDatos _acceso;
    private readonly ILogger<ClienteController> _logger;

    public ClienteController(AccesoDatos acceso, ILogger<ClienteController> logger)
    {
        _acceso = acceso;
        _logger = logger;
    }

    [HttpGet("Perfil")]
    public IActionResult Perfil()
    {
        try
        {
            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null)
            {
                _logger.LogWarning("Intento de acceso a perfil sin sesión");
                return RedirectToAction("InicioSesion", "Auth");
            }

            var modelo = _acceso.ObtenerPerfilUsuario(idUsuario.Value);
            if (modelo == null)
            {
                _logger.LogError($"No se encontró perfil para el usuario {idUsuario}");
                return RedirectToAction("InicioSesion", "Auth");
            }

            modelo.Usuario ??= new Usuario();
            modelo.Persona ??= new Persona();
            modelo.Direccion ??= new Direccion();
            modelo.Cliente ??= new Cliente();

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
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modelo inválido en GuardarPerfil");
                return BadRequest(ModelState);
            }

            int? idUsuario = HttpContext.Session.GetInt32("IdUsuario");
            if (idUsuario == null || model.Usuario?.ID != idUsuario)
            {
                _logger.LogWarning("Intento de actualización con sesión inválida");
                return Unauthorized();
            }

            bool actualizado = _acceso.ActualizarPerfilUsuario(model);
            if (!actualizado)
            {
                _logger.LogError("No se pudo actualizar el perfil en la base de datos");
                return StatusCode(500);
            }

            _logger.LogInformation($"Perfil actualizado para usuario {idUsuario}");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar perfil");
            return StatusCode(500);
        }
    }
}