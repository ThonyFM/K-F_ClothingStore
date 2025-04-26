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
builder.Services.AddHttpContextAccessor(); // 👈 ESTA ES LA LÍNEA CLAVE

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

app.UseSession(); // 👈 ¡Debe ir antes que Authorization!
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=InicioSesion}/{id?}");

app.Run();