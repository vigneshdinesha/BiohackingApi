using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiohackingApi.Web.Models;

[Table("motivations")]
public class Motivation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("createddate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Column("updateddate")]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<MotivationBiohack> MotivationBiohacks { get; set; } = new List<MotivationBiohack>();
}
