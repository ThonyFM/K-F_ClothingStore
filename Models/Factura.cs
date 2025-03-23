namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Factura {
        public Factura() {}
        [Key]
        public int ID { get; set; }

        [Required]
        public int ClienteID { get; set; }
        
        public DateTime FechaEmision { get; set; } = DateTime.Now;

        [Required]
        public decimal Total { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string MetodoPago { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Estado { get; set; }
        
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string CreadoPor { get; set; }

        [StringLength(maximumLength: 50)]
        public string ModificadoPor { get; set; }
    }
}
