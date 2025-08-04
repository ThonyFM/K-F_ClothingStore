using System.Diagnostics;
using K_F_ClothingStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace K_F_ClothingStore.Controllers;

public class HomeController : Controller
{
    private readonly AccesoDatos _acceso;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, AccesoDatos acceso)
    {
        _logger = logger;
        _acceso = acceso;
    }

    public IActionResult Registro()
    {
        return View();
    }

    public IActionResult Index()
    {
        List<Producto> listaProductos = _acceso.ObtenerTodosLosProductos();
        return View(listaProductos);
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}