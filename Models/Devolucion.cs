using System.ComponentModel.DataAnnotations;

namespace K_F_ClothingStore.Models
{
    public class Devolucion
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int FacturaID { get; set; }

        [Required]
        public int DetalleFacturaID { get; set; }

        [Required]
        public int ProductoID { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [StringLength(255)]
        public string Motivo { get; set; }

        public DateTime FechaDevolucion { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }

        [Required]
        [StringLength(50)]
        public string CreadoPor { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [StringLength(50)]
        public string ModificadoPor { get; set; }

        public Devolucion() { }

        public Devolucion(int id, int facturaID, int detalleFacturaID, int productoID, int cantidad, string motivo, string estado, string creadoPor)
        {
            ID = id;
            FacturaID = facturaID;
            DetalleFacturaID = detalleFacturaID;
            ProductoID = productoID;
            Cantidad = cantidad;
            Motivo = motivo;
            Estado = estado;
            CreadoPor = creadoPor;
        }
    }
}