using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Entities;

public class AddressDelivery
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public virtual ApplicationUser User { get; set; }
    
    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    [Phone]
    public string Phone { get; set; }
    
    [MaxLength(100)]
    public string Street { get; set; }
    
    [MaxLength(10)]
    public string HouseNumber { get; set; }

    [MaxLength(50)]
    public string City { get; set; }

    public int ZipCode { get; set; }
    
    [MaxLength(200)]
    public string? Note { get; set; }
}