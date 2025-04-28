
namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Cliente {
        public Cliente() {}

        public Cliente(int id, int personaID, int codigoCliente, string estado, string creadoPor)
        {
            ID = id;
            PersonaID = personaID;
            CodigoCliente = codigoCliente;
            Estado = estado;
            CreadoPor = creadoPor;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public int PersonaID { get; set; }

        [Required]
        public int CodigoCliente { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string CreadoPor { get; set; } = "Admin";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [StringLength(maximumLength: 50)]
        public string ModificadoPor { get; set; }
    }
}
