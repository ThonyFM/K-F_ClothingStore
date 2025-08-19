using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace K_F_ClothingStore.Models;

public class Usuario
{
    [Key] public int ID { get; set; }

    [Required] [StringLength(50)] public string? NombreUsuario { get; set; }

    [Required]
    [StringLength(255)]
    [ValidateNever] // ✅ No se edita en el formulario de Perfil
    public string ContrasenaHash { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; }

    [Required] [StringLength(50)] public string Rol { get; set; } = "Cliente";

    [Required] [StringLength(50)] public string Estado { get; set; } = "Activo";

    public DateTime? FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaModificacion { get; set; }
}