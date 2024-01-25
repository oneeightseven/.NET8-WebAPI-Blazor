using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mgeek.Services.ProductAPI.Models;

[Table("Smartphones")]
public class Smartphone : Product
{
    [Required]
    public int Ram { get; set; }
    [Required]
    public int Rom { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string? Cpu { get; set; }
    [Required]
    public double Diagonal { get; set; }
    [Required]
    public int CameraPixels { get; set; }
    [Required]
    public int Battery { get; set; }
}