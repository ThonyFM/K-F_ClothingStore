namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Empleado {
        public Empleado() {}

        public Empleado(int id, int personaID, string puesto, DateTime fechaContratacion, decimal salario, string estado, string creadoPor)
        {
            ID = id;
            PersonaID = personaID;
            Puesto = puesto;
            FechaContratacion = fechaContratacion;
            Salario = salario;
            Estado = estado;
            CreadoPor = creadoPor;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public int PersonaID { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Puesto { get; set; }

        [Required]
        public DateTime FechaContratacion { get; set; }

        [Required]
        public decimal Salario { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string CreadoPor { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [StringLength(maximumLength: 50)]
        public string ModificadoPor { get; set; }
    }
}
