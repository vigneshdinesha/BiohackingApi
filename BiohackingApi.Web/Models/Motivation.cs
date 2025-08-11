using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BiohackingApi.Web.Models;

[Table("motivations")]
public class Motivation
{
    [System.ComponentModel.DataAnnotations.Schema.Column("id")]
    public int Id { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("title")]
    public string Title { get; set; } = null!;
    [System.ComponentModel.DataAnnotations.Schema.Column("description")]
    public string? Description { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("createddate")]
    public DateTime CreatedDate { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("updateddate")]
    public DateTime UpdatedDate { get; set; }

    public ICollection<MotivationBiohack>? MotivationBiohacks { get; set; }
}
