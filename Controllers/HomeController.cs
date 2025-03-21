namespace K_F_ClothingStore.Controllers {
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using System.Diagnostics;
    using System.Runtime.InteropServices.ComTypes;

    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly AccesoDatos _acceso;   
   
        public HomeController(ILogger<HomeController> logger, AccesoDatos acceso)
        {
            _logger = logger;
            _acceso = acceso;
        }
        public IActionResult Registro() => View();
        
        public IActionResult Index()
        {
            List<Producto> listaProductos = _acceso.ObtenerTodosLosProductos();
            return View(listaProductos);
        }
       
        
        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
