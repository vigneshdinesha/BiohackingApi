using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiohackingApi.Web.Models;

[Table("journals")]
public class Journal
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("userid")]
    public int UserId { get; set; }

    [Required]
    [Column("biohackid")]
    public int BiohackId { get; set; }

    [Column("notes")]
    public string? Notes { get; set; }

    [Range(1, 10)]
    [Column("rating")]
    public int? Rating { get; set; }

    [Column("datetime")]
    public DateTime DateTime { get; set; }

    [Column("createddate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Column("updateddate")]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("BiohackId")]
    public virtual Biohack Biohack { get; set; } = null!;
}
