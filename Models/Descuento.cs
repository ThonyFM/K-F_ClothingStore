namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Descuento {
        public Descuento() {}

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

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Tipo { get; set; }

        [Required]
        public decimal Valor { get; set; }

        [StringLength(maximumLength: 255)]
        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Estado { get; set; }
    }
}
