namespace K_F_ClothingStore.Controllers {
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Diagnostics;
    public class AuthController : Controller {
        private readonly AccesoDatos _acceso;
        public AuthController(AccesoDatos acceso)
        {
            _acceso = acceso;
        }
        // GET: Auth/Registro
        public IActionResult Registro() => View();
        // POST: Auth/Registro
        [HttpPost]
        public IActionResult Registro(RegistroViewModel model)
        {
            bool seguro = false;
            try
            {
                _acceso.ObtenerPersonaPorCedula(model.Persona.DocumentoIdentidad);

            }
            catch (Exception)
            {
                seguro = true;
                TempData["ErrorMessage"] = "El documento de indentidad ingresado ya pertenece a otra cuenta.";
            }
            try
            {
                _acceso.ObtenerPersonaPorTelefono(model.Persona.Telefono);

            }
            catch (Exception)
            {
                seguro = true;
                TempData["ErrorMessage"] = "El telefono ingresado ya pertenece a otra cuenta.";
            }
            if (_acceso.ObtenerUsuarioPorEmail(model.Usuario.Email) != null)
            {
                seguro = true;
                TempData["ErrorMessage"] = "El Email ingresado ya pertenece a otra cuenta.";
            }
            if (_acceso.ObtenerUsuarioPorNombreUsuario(model.Usuario.NombreUsuario) != null)
            {
                seguro = true;
                TempData["ErrorMessage"] = "El Nombre de usuario ingresado ya pertenece a otra cuenta.";
            }
            if (seguro == false)
            {
                Usuario? usuario = new Usuario
                {
                    NombreUsuario = model.Usuario.NombreUsuario,
                    ContrasenaHash = model.Usuario.ContrasenaHash,
                    Email = model.Usuario.Email,
                    Rol = "Cliente",
                    Estado = "Activo"

                };
                _acceso.AgregarUsuario(usuario);
                Direccion? direccion = new Direccion
                {
                    Ciudad = model.Direccion.Ciudad,
                    Estado = model.Direccion.Estado,
                    CodigoPostal = model.Direccion.CodigoPostal,
                    Pais = model.Direccion.Pais,
                    TipoDireccion = model.Direccion.TipoDireccion,
                    CreadoPor = "Sistema"
                };
                _acceso.AgregarDireccion(direccion);
                Persona? persona = new Persona
                {
                    Nombre1 = model.Persona.Nombre1,
                    Nombre2 = model.Persona.Nombre2,
                    Apellido1 = model.Persona.Apellido1,
                    Apellido2 = model.Persona.Apellido2,
                    DocumentoIdentidad = model.Persona.DocumentoIdentidad,
                    Telefono = model.Persona.Telefono,
                    Email = model.Usuario.Email,
                    FechaNacimiento = model.Persona.FechaNacimiento,
                    Genero = model.Persona.Genero,
                    DireccionID = direccion.ID,
                    CreadoPor = "Sistema"
                };
                _acceso.AgregarPersona(persona);
                Cliente? cliente = new Cliente
                {
                    PersonaID = persona.ID,
                    CodigoCliente = direccion.ID,
                    Estado = "Activo",
                    CreadoPor = "Sistema"
                };
                _acceso.AgregarCliente(cliente);
                TempData["SuccessMessage"] = "Usuario registrado exitosamente.";
                return RedirectToAction("InicioSesion", "Auth");

            }
            return View(model);
        }
        // GET: Auth/InicioSesion
        public IActionResult InicioSesion() => View();
        // POST: Auth/InicioSesion
        [HttpPost]
        public IActionResult InicioSesion(string? nombreUsuario, string contrasenaHash)
        {
            
                Usuario? usuario = _acceso.ObtenerUsuarioPorNombreUsuario(nombreUsuario);
                if (usuario == null || usuario.ContrasenaHash != contrasenaHash)
                {
                    TempData["ErrorMessage"] = "Nombre de usuario o contraseña incorrectos.";
                    return View();
                }
                // Guardar el usuario en la sesión
                Persona? persona = _acceso.ObtenerPersonaPorEmail(usuario.Email);
                Direccion? direccion = _acceso.ObtenerDireccionPorId(persona.DireccionID);
                HttpContext.Session.SetString("TipoDireccion", direccion.TipoDireccion.ToString());
                HttpContext.Session.SetString("PersonaID", persona.ID.ToString());
                HttpContext.Session.SetString("Nombre", persona.Nombre1+" "+persona.Apellido1+" "+persona.Apellido2);
                HttpContext.Session.SetString("UsuarioID", usuario.ID.ToString());
                HttpContext.Session.SetString("Telefono", persona.Telefono);
                HttpContext.Session.SetString("Email", usuario.Email);
                HttpContext.Session.SetString("Usuario", usuario.NombreUsuario);
                HttpContext.Session.SetString("Rol", usuario.Rol);
                TempData["SuccessMessage"] = "Inicio de sesión exitoso.";
                return RedirectToAction("Index", "Home");
           
        }
        // GET: Auth/CerrarSesion
        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "Sesión cerrada correctamente.";
            return RedirectToAction("InicioSesion");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
