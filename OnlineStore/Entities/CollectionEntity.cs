using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

[Table("Collection")]
public class CollectionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Title { get; set; }
    
    public virtual ICollection<CollectionEntityGood> CollectionGoods { get; set; }
}