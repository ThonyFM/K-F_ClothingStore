using K_F_ClothingStore.Models;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Logging personalizado
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile("Logs/kf-log-{Date}.txt");

// Servicios
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<AccesoDatos>();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor(); 

// Configuración de autenticación con cookies
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/InicioSesion"; // Ruta de inicio de sesión
        options.LogoutPath = "/Auth/CerrarSesion"; // Ruta de cierre de sesión
    });

// Configuración de sesión (si es necesario)
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".K_F_ClothingStore.Session"; // Nombre de la cookie de sesión
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Timeout de sesión
    options.Cookie.HttpOnly = true; // Seguridad de la cookie
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();  // Habilitar sesiones
app.UseAuthentication(); // Habilitar autenticación
app.UseAuthorization();  // Habilitar autorización

// Definir las rutas
app.MapControllerRoute(
    name: "admin",
    pattern: "Admin/{action=Productos}/{id?}",
    defaults: new { controller = "Admin" });
app.MapControllerRoute(
    name: "cliente",
    pattern: "Cliente/{action=Perfil}/{id?}",
    defaults: new { controller = "Cliente" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=InicioSesion}/{id?}");

app.Run();