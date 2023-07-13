using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Entities;


public class Good
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    
    public virtual Category Category { get; set; }
    
    public DateTimeOffset TimePublication { get; set; }
    
    [MaxLength(1000)]
    public string Description { get; set; }
    
    public ICollection<SubGood> SubGoods { get; set; } 
    public ICollection<Response> Responses { get; set; } 
}