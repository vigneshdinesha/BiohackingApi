using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiohackingApi.Web.Models;

[Table("motivation_biohacks")]
public class MotivationBiohack
{
    [System.ComponentModel.DataAnnotations.Schema.Column("motivation_id")]
    public int MotivationId { get; set; }
    public Motivation Motivation { get; set; } = null!;

    [System.ComponentModel.DataAnnotations.Schema.Column("biohack_id")]
    public int BiohackId { get; set; }
    public Biohack Biohack { get; set; } = null!;
}
