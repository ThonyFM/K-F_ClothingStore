using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace K_F_ClothingStore.Models;
// Necesario para [NotMapped]

public class Factura
{
    [Key] public int ID { get; set; }

    [Required] public int ClienteID { get; set; }

    [Required] public decimal Total { get; set; }

    [Required] [StringLength(50)] public string MetodoPago { get; set; }

    [Required] [StringLength(50)] public string Estado { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? FechaModificacion { get; set; }

    [Required] [StringLength(50)] public string CreadoPor { get; set; }

    [StringLength(50)] public string ModificadoPor { get; set; }

    // ⚡ Este sí lo dejamos: Solo presentación
    [NotMapped] public DateTime FechaEmision => FechaCreacion;
}