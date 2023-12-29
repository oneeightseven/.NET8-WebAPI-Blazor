using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mgeek.Services.PromocodeAPI.Models;

[Table("Promocodes")]
public class Promocode
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string? Name { get; set; }
    [Required]
    public int SumDiscount { get; set; }
    [Required]
    public int Remainder { get; set; }
    [Required]
    public int MinAmount { get; set; }
}