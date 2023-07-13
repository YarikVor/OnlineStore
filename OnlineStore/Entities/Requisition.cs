using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Entities;

public class Requisition
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey(nameof(ApplicationUser))]
    public int UserId { get; set; }
    
    
    public ApplicationUser ApplicationUser { get; set; }
    
    [MaxLength(50)]
    public string FirstName { get; set; }
    
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [EmailAddress]
    [MaxLength(100)] 
    public string Email { get; set; }
    
    [Phone]
    public string Phone { get; set; }
    
    [ForeignKey(nameof(DeliveryMethod))]
    public int DeliveryMethodId { get; set; }
    
    public DeliveryMethod DeliveryMethod { get; set; }
    
    [MaxLength(50)]
    public string City { get; set; }
    
    [MaxLength(50)]
    public string PostOffset { get; set; }
    
    public DateTimeOffset TimeWriting { get; set; }
    
    public bool Payed { get; set; }
    
    public ICollection<RequisitionGood> RequisitionGoods { get; set; }
}