using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Descuento
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public Descuento() { }

        public Descuento(int id, string tipo, decimal valor, string descripcion, DateTime fechaInicio, DateTime? fechaFin, string estado)
        {
            ID = id;
            Tipo = tipo;
            Valor = valor;
            Descripcion = descripcion;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
            Estado = estado;
        }
    }
}