using Microsoft.AspNetCore.Mvc;
using K_F_ClothingStore.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace K_F_ClothingStore.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _conexion;

        public ProductoController(IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _conexion = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: Producto/Index
        public IActionResult Index()
        {
            var producto = ObtenerTodosproducto();
            return View(producto);
        }

        private List<Producto> ObtenerTodosproducto()
        {
            List<Producto> producto = new List<Producto>();
            using (var con = new SqlConnection(_conexion))
            {
                var query = "Exec sp_GetAllproducto";
                using (var cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            producto.Add(new Producto
                            {
                                ID = reader.GetInt32(0),
                                NombreProducto = reader.GetString(1),
                                Genero = reader.GetString(2),
                                SegmentoEdad = reader.GetString(3),
                                TipoProducto = reader.GetString(4),
                                Color = reader.GetString(5),
                                Talla = reader.GetString(6),
                                UnidadesDisponibles = reader.GetInt32(7),
                                Precio = reader.GetDecimal(8),
                                ImagenUrl = reader.IsDBNull(9) ? null : reader.GetString(9),
                                Descripcion = reader.IsDBNull(10) ? null : reader.GetString(10),
                                FechaCreacion = reader.GetDateTime(11),
                                FechaModificacion = reader.IsDBNull(12) ? null : reader.GetDateTime(12)
                            });
                    }
                }
            }
            return producto;
        }

        // GET: Producto/Details/5
        public IActionResult Details(int id)
        {
            var producto = ObtenerProductoPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            return PartialView("_DetailsModal", producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            return PartialView("_CreateModal", new Producto());
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                if (producto.ImagenArchivo != null && producto.ImagenArchivo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "producto");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + producto.ImagenArchivo.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await producto.ImagenArchivo.CopyToAsync(fileStream);
                    }

                    producto.ImagenUrl = "/images/producto/" + uniqueFileName;
                }

                using (var con = new SqlConnection(_conexion))
                {
                    var query = "Exec sp_InsertProducto @NombreProducto, @Genero, @SegmentoEdad, @TipoProducto, @Color, @Talla, @UnidadesDisponibles, @Precio, @Descripcion, @ImagenUrl";
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                        cmd.Parameters.AddWithValue("@Genero", producto.Genero);
                        cmd.Parameters.AddWithValue("@SegmentoEdad", producto.SegmentoEdad);
                        cmd.Parameters.AddWithValue("@TipoProducto", producto.TipoProducto);
                        cmd.Parameters.AddWithValue("@Color", producto.Color);
                        cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                        cmd.Parameters.AddWithValue("@UnidadesDisponibles", producto.UnidadesDisponibles);
                        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ImagenUrl", producto.ImagenUrl ?? (object)DBNull.Value);
                        
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return Json(new { success = true });
            }

            return PartialView("_CreateModal", producto);
        }

        // GET: Producto/Edit/5
        public IActionResult Edit(int id)
        {
            var producto = ObtenerProductoPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            return PartialView("_EditModal", producto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (producto.ImagenArchivo != null && producto.ImagenArchivo.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images", "producto");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + producto.ImagenArchivo.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await producto.ImagenArchivo.CopyToAsync(fileStream);
                    }

                    producto.ImagenUrl = "/images/producto/" + uniqueFileName;
                }

                using (var con = new SqlConnection(_conexion))
                {
                    var query = "Exec sp_UpdateProducto @ID, @NombreProducto, @Genero, @SegmentoEdad, @TipoProducto, @Color, @Talla, @UnidadesDisponibles, @Precio, @Descripcion, @ImagenUrl";
                    using (var cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", producto.ID);
                        cmd.Parameters.AddWithValue("@NombreProducto", producto.NombreProducto);
                        cmd.Parameters.AddWithValue("@Genero", producto.Genero);
                        cmd.Parameters.AddWithValue("@SegmentoEdad", producto.SegmentoEdad);
                        cmd.Parameters.AddWithValue("@TipoProducto", producto.TipoProducto);
                        cmd.Parameters.AddWithValue("@Color", producto.Color);
                        cmd.Parameters.AddWithValue("@Talla", producto.Talla);
                        cmd.Parameters.AddWithValue("@UnidadesDisponibles", producto.UnidadesDisponibles);
                        cmd.Parameters.AddWithValue("@Precio", producto.Precio);
                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ImagenUrl", producto.ImagenUrl ?? (object)DBNull.Value);
                        
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return Json(new { success = true });
            }

            return PartialView("_EditModal", producto);
        }

        // GET: Producto/Delete/5
        public IActionResult Delete(int id)
        {
            var producto = ObtenerProductoPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteModal", producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (var con = new SqlConnection(_conexion))
            {
                var query = "Exec sp_DeleteProducto @ID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return Json(new { success = true });
        }

        private Producto ObtenerProductoPorId(int id)
        {
            Producto producto = null;
            using (var con = new SqlConnection(_conexion))
            {
                var query = "Exec sp_GetProductoById @ID";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            producto = new Producto
                            {
                                ID = reader.GetInt32(0),
                                NombreProducto = reader.GetString(1),
                                Genero = reader.GetString(2),
                                SegmentoEdad = reader.GetString(3),
                                TipoProducto = reader.GetString(4),
                                Color = reader.GetString(5),
                                Talla = reader.GetString(6),
                                UnidadesDisponibles = reader.GetInt32(7),
                                Precio = reader.GetDecimal(8),
                                ImagenUrl = reader.IsDBNull(9) ? null : reader.GetString(9),
                                Descripcion = reader.IsDBNull(10) ? null : reader.GetString(10),
                                FechaCreacion = reader.GetDateTime(11),
                                FechaModificacion = reader.IsDBNull(12) ? null : reader.GetDateTime(12)
                            };
                    }
                }
            }
            return producto;
        }
    }
}