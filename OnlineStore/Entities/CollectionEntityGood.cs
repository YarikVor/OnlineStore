using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

[Table("CollectionGood")]
public class CollectionEntityGood
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Collection))]
    public int CollectionId { get; set; }
    
    public virtual CollectionEntity Collection { get; set; }
    
    [ForeignKey(nameof(Good))]
    public int GoodId { get; set; }
    public virtual Good Good { get; set; }

}