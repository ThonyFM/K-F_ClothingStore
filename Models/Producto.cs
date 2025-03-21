namespace K_F_ClothingStore.Models {
    using System.ComponentModel.DataAnnotations;

    public class Producto {
        public Producto() {}

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string NombreProducto { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string Genero { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string SegmentoEdad { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string TipoProducto { get; set; }

        [Required]
        [StringLength(maximumLength: 30)]
        public string Color { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string Talla { get; set; }

        [Required]
        public int UnidadesDisponibles { get; set; }

        [Required]
        public decimal Precio { get; set; }
        [Required]
        public string ImagenUrl { get; set; }
        [StringLength(maximumLength: 500)]
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }
        public Producto(int id, string nombreProducto, string genero, string segmentoEdad, string tipoProducto, string color, string talla, int unidadesDisponibles, decimal precio, string imagenUrl, string descripcion, DateTime? fechaModificacion)
        {
            ID = id;
            NombreProducto = nombreProducto;
            Genero = genero;
            SegmentoEdad = segmentoEdad;
            TipoProducto = tipoProducto;
            Color = color;
            Talla = talla;
            UnidadesDisponibles = unidadesDisponibles;
            Precio = precio;
            ImagenUrl = imagenUrl;
            Descripcion = descripcion;
            FechaModificacion = fechaModificacion;  
        }
    }
}
