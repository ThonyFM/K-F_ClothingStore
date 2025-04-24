namespace K_F_ClothingStore.Models {
    public class RegistroViewModel
    {
        public Usuario Usuario { get; set; } = new Usuario();
        public Direccion Direccion { get; set; } = new Direccion();
        public Persona Persona { get; set; } = new Persona();
        public Cliente Cliente { get; set; } = new Cliente();
    }
}
