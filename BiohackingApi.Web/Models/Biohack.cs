using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BiohackingApi.Web.Models;

[Table("biohacks")]
public class Biohack
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("info_sections", TypeName = "jsonb")]
    public string? InfoSections { get; set; }

    [Column("createddate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [Column("updateddate")]
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
    public virtual ICollection<MotivationBiohack> MotivationBiohacks { get; set; } = new List<MotivationBiohack>();

    // Helper property to work with JSON data
    [NotMapped]
    public JsonDocument? InfoSectionsJson
    {
        get => string.IsNullOrEmpty(InfoSections) ? null : JsonDocument.Parse(InfoSections);
        set => InfoSections = value?.RootElement.GetRawText();
    }
}
