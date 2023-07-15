using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class Response
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(ApplicationUser))]
    public int UserId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; }

    public int GoodId { get; set; }

    public virtual Good Good { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(1024)]
    public string Description { get; set; }

    public DateTimeOffset TimeWriting { get; set; }
}