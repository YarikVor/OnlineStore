using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class Favourite
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; }
    
    
    [ForeignKey(nameof(Good))]
    public int GoodId { get; set; }
    public virtual Good Good { get; set; }
    

}