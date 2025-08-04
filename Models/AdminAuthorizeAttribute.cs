using Microsoft.AspNetCore.Mvc;

namespace K_F_ClothingStore.Models;

public class AdminAuthorizeAttribute : TypeFilterAttribute
{
    public AdminAuthorizeAttribute() : base(typeof(AdminFilter))
    {
    }
}