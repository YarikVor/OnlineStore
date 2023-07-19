using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class Blog
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public DateTimeOffset TimeWriting { get; set; }

    [Url]
    [MaxLength(100)]
    public string PhotoUri { get; set; } = null!;

    [MaxLength(100)]
    public string Title { get; set; } = null!;

    [MaxLength(300)]
    public string Description { get; set; } = null!;

    [MaxLength(100)]
    public string ContentUri { get; set; } = null!;
}