namespace K_F_ClothingStore.Models
{
    public class ContexDB
    {
        private readonly string _conexion;

        public ContexDB(IConfiguration configuration)
        {
            _conexion = configuration.GetConnectionString("Conexion");
        }

    }
}
