namespace K_F_ClothingStore.Models
{
    public class Cliente
    {
        public int ID { get; set; }
        public int PersonaID { get; set; }
        public int CodigoCliente { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string CreadoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string ModificadoPor { get; set; }
        public Persona Persona { get; set; } // Relación con Persona
    }
}