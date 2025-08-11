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
    [System.ComponentModel.DataAnnotations.Schema.Column("firstname")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [System.ComponentModel.DataAnnotations.Schema.Column("lastname")]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [System.ComponentModel.DataAnnotations.Schema.Column("email")]
    public string Email { get; set; } = string.Empty;

    [System.ComponentModel.DataAnnotations.Schema.Column("provider")]
    public string? Provider { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Column("externalid")]
    public string? ExternalId { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Column("subid")]
    public string? SubId { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Column("motivationid")]
    public int? MotivationId { get; set; }

    [System.ComponentModel.DataAnnotations.Schema.Column("createddate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [System.ComponentModel.DataAnnotations.Schema.Column("updateddate")]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("MotivationId")]
    public virtual Motivation? Motivation { get; set; }

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}
