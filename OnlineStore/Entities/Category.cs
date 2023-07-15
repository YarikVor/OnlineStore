using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int ParentId { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string OriginalName { get; set; }

    [Url]
    public string PhotoUri { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }
}