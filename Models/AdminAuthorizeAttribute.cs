namespace K_F_ClothingStore.Models {
    using Microsoft.AspNetCore.Mvc;

    public class AdminAuthorizeAttribute : TypeFilterAttribute {
        public AdminAuthorizeAttribute() : base(typeof(AdminFilter))
        {
        }
    }
}
