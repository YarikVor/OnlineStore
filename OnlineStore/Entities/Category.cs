using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(Parent))]
    public int? ParentId { get; set; } = null;
    
    public virtual Category? Parent { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string OriginalName { get; set; }

    [Url]
    public string PhotoUri { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [ForeignKey(nameof(ParentId))]
    public virtual ICollection<Category> Children { get; set; }
}