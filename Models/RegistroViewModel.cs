namespace K_F_ClothingStore.Models;

public class RegistroViewModel
{
    public Usuario Usuario { get; set; } = new();
    public Direccion Direccion { get; set; } = new();
    public Persona Persona { get; set; } = new();
    public Cliente Cliente { get; set; } = new();
}