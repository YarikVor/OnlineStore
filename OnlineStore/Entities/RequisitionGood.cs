using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class RequisitionGood
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(Entities.Requisition))]
    public int RequisitionId { get; set; }
    
    public virtual Requisition Requisition { get; set; }
   
    [ForeignKey(nameof(SubGood))]
    public int SubGoodId { get; set; }
    
    public virtual SubGood SubGood { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Count { get; set; }
}