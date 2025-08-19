using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace K_F_ClothingStore.Controllers;

[Route("Producto")]
public class ProductoController : Controller
{
    private readonly AccesoDatos _acceso;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ProductoController> _logger;

    public ProductoController(AccesoDatos acceso, IWebHostEnvironment env, ILogger<ProductoController> logger)
    {
        _acceso = acceso;
        _env = env;
        _logger = logger;
    }

    // GET: /Producto
    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        var productos = _acceso.ObtenerTodosLosProductos();
        return View(productos); // <- tu Index.cshtml unificado espera IEnumerable<Producto>
    }

    // POST: /Producto/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create([FromForm] Producto model)
    {
        try
        {
            // Manejo de imagen (opcional)
            if (model.ImagenArchivo != null && model.ImagenArchivo.Length > 0)
            {
                var fileName = Path.GetFileName(model.ImagenArchivo.FileName); // solo nombre
                var savePath = Path.Combine(_env.WebRootPath ?? "wwwroot", "Img");
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                var fullPath = Path.Combine(savePath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.ImagenArchivo.CopyTo(stream);
                }

                model.ImagenUrl = fileName; // <- SOLO nombre en BD
            }

            _acceso.AgregarProducto(model);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear producto");
            return Json(new { success = false, message = "Error al crear el producto." });
        }
    }

    // POST: /Producto/Edit/{id}
    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [FromForm] Producto model)
    {
        try
        {
            // Traer actual para conservar imagen si no suben una nueva
            var actual = _acceso.ObtenerProductoPorId(id);
            if (actual == null)
                return Json(new { success = false, message = "Producto no encontrado." });

            model.ID = id;

            if (model.ImagenArchivo != null && model.ImagenArchivo.Length > 0)
            {
                var fileName = Path.GetFileName(model.ImagenArchivo.FileName);
                var savePath = Path.Combine(_env.WebRootPath ?? "wwwroot", "Img");
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);

                var fullPath = Path.Combine(savePath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.ImagenArchivo.CopyTo(stream);
                }

                model.ImagenUrl = fileName; // nuevo nombre
            }
            else
            {
                // conservar la anterior
                model.ImagenUrl = actual.ImagenUrl;
            }

            _acceso.ActualizarProducto(model);
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al editar producto {Id}", id);
            return Json(new { success = false, message = "Error al actualizar el producto." });
        }
    }

    // POST: /Producto/Delete/{id}
    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try
        {
            _acceso.EliminarProducto(id);
            // Si deseas borrar el archivo físico, aquí podrías:
            // var p = _acceso.ObtenerProductoPorId(id); (hacerlo antes de eliminar)
            // y luego File.Delete(Path.Combine(_env.WebRootPath, "Img", p.ImagenUrl)) con verificaciones.

            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar producto {Id}", id);
            return Json(new { success = false, message = "Error al eliminar el producto." });
        }
    }
}