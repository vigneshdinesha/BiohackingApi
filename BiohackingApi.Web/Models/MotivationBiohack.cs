using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiohackingApi.Web.Models;

[Table("motivation_biohacks")]
public class MotivationBiohack
{
    [Key, Column("motivation_id", Order = 0)]
    public int MotivationId { get; set; }

    [Key, Column("biohack_id", Order = 1)]
    public int BiohackId { get; set; }

    // Navigation properties
    [ForeignKey("MotivationId")]
    public virtual Motivation Motivation { get; set; } = null!;

    [ForeignKey("BiohackId")]
    public virtual Biohack Biohack { get; set; } = null!;
}
