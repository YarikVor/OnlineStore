using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class SubGood
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Good))]
    public int GoodId { get; set; }
    
    public Good Good { get; set; }
    
    [Url]
    [MaxLength(100)]
    public string PhotoUri { get; set; }
    
    public decimal PriceUah { get; set; }
    
    [MaxLength(20)]
    public string Acticle { get; set; }
    
    [MaxLength(20)]
    public string Size { get; set; }
    
    [MaxLength(20)]
    public string Color { get; set; }
    
    public decimal Discount { get; set; }
}