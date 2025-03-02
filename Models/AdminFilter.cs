using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace K_F_ClothingStore.Models
{
    public class AdminFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("Rol") != "Administrador")
            {
                context.Result = new RedirectToActionResult("Login", "Usuario", null);
            }
        }
    }
}