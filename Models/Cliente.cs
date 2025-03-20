using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Cliente
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PersonaID { get; set; }

        [Required]
        public int CodigoCliente { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(50)]
        public string CreadoPor { get; set; } ="Admin";

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [StringLength(50)]
        public string ModificadoPor { get; set; }

        public Cliente() { }

        public Cliente(int id, int personaID, int codigoCliente, string estado, string creadoPor)
        {
            ID = id;
            PersonaID = personaID;
            CodigoCliente = codigoCliente;
            Estado = estado;
            CreadoPor = creadoPor;
        }
    }
}