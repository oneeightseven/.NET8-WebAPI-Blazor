using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mgeek.Services.ProductAPI.Models;
[Table("Stocks")]
public class Stock 
{
    [Key]
    public int ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product? Product { get; set; }
    [Required]
    [Range(0,1000)]
    public int Amount { get; set; }
}