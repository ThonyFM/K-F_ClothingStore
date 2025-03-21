namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Direccion {
        public Direccion() {}

        public Direccion(int id, string ciudad, string estado, string codigoPostal, string pais, string tipoDireccion, string creadoPor)
        {
            ID = id;
            Ciudad = ciudad;
            Estado = estado;
            CodigoPostal = codigoPostal;
            Pais = pais;
            TipoDireccion = tipoDireccion;
            CreadoPor = creadoPor;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Ciudad { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string CodigoPostal { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Pais { get; set; }

        [StringLength(maximumLength: 20)]
        public string TipoDireccion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(maximumLength: 50)]
        public string CreadoPor { get; set; } = "Admin";

        public DateTime? FechaModificacion { get; set; }

        [StringLength(maximumLength: 50)]
        public string ModificadoPor { get; set; }
    }
}
