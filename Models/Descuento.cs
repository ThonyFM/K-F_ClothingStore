namespace K_F_ClothingStore.Models
{
    public class Descuento
    {
        public int ID { get; set; }
        public string Tipo { get; set; }
        public decimal Valor { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; }
    }
}