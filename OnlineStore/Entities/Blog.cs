using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class Blog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(ApplicationUser))]
    public int UserId { get; set; }
    
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    
    public DateTimeOffset TimeWriting { get; set; }
    
    [Url]
    public string PhotoUri { get; set; } = null!;
    
    [MaxLength(100)]
    public string Title { get; set; } = null!;
    
    [MaxLength(3000)]
    public string Description { get; set; } = null!;
}