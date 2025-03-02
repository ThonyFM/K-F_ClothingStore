using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Factura
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int ClienteID { get; set; }

        [Required]
        public int EmpleadoID { get; set; }

        public DateTime FechaEmision { get; set; } = DateTime.Now;

        [Required]
        public decimal Total { get; set; }

        [Required]
        [StringLength(50)]
        public string MetodoPago { get; set; }

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        public int? DescuentoID { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [Required]
        [StringLength(50)]
        public string CreadoPor { get; set; }

        [StringLength(50)]
        public string ModificadoPor { get; set; }

        public Factura() { }

        public Factura(int id, int clienteID, int empleadoID, decimal total, string metodoPago, string estado, int? descuentoID, string creadoPor)
        {
            ID = id;
            ClienteID = clienteID;
            EmpleadoID = empleadoID;
            Total = total;
            MetodoPago = metodoPago;
            Estado = estado;
            DescuentoID = descuentoID;
            CreadoPor = creadoPor;
        }
    }
}