using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace K_F_ClothingStore.Models;

public class Persona
{
    [Key] public int ID { get; set; }

    [Required, StringLength(50)]
    public string Nombre1 { get; set; }

    [StringLength(50)]
    public string Nombre2 { get; set; }

    [Required, StringLength(50)]
    public string Apellido1 { get; set; }

    [Required, StringLength(50)]
    public string Apellido2 { get; set; }

    [Required, StringLength(20)]
    [ValidateNever]                  // ✅ No se edita en Perfil
    public string DocumentoIdentidad { get; set; }

    [StringLength(15)]
    public string Telefono { get; set; }

    [EmailAddress, StringLength(255)]
    public string Email { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    [StringLength(50)]
    public string Genero { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [Required, StringLength(50)]
    [ValidateNever]                  // ✅ No lo mandas en Perfil
    public string CreadoPor { get; set; }

    public DateTime? FechaModificacion { get; set; }
    [StringLength(50)] public string ModificadoPor { get; set; }

    public int DireccionID { get; set; }
    public int? UsuarioID { get; set; }
}