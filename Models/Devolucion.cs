namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Devolucion {
        public Devolucion() {}

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
        [StringLength(maximumLength: 255)]
        public string Motivo { get; set; }

        public DateTime FechaDevolucion { get; set; } = DateTime.Now;

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
