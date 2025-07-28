using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiohackingApi.Web.Models;

[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("firstname")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Column("lastname")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("provider")]
    public string? Provider { get; set; }

    [Column("externalid")]
    public string? ExternalId { get; set; }

    [Column("subid")]
    public string? SubId { get; set; }

    [Column("motivationid")]
    public int? MotivationId { get; set; }

    [Column("createddate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Column("updateddate")]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("MotivationId")]
    public virtual Motivation? Motivation { get; set; }

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}
