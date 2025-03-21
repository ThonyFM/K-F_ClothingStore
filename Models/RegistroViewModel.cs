namespace K_F_ClothingStore.Models {
    public class RegistroViewModel {
        public RegistroViewModel()
        {

        }
        public RegistroViewModel(Usuario usuario, Direccion direccion, Persona persona, Cliente cliente)
        {
            Usuario = usuario;
            Direccion = direccion;
            Persona = persona;
            Cliente = cliente;
        }
        public Usuario Usuario { get; set; }
        public Direccion Direccion { get; set; }
        public Persona Persona { get; set; }
        public Cliente Cliente { get; set; }
    }
}
