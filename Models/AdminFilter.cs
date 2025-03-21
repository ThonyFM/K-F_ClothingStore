namespace K_F_ClothingStore.Models {
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class AdminFilter : IAuthorizationFilter {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("Rol") != "Administrador")
            {
                context.Result = new RedirectToActionResult("Login", "Usuario", routeValues: null);
            }
        }
    }
}
