using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace BiohackingApi.Web.Models;

public enum BiohackCategory
{
    WellnessAndBalance,
    PerformanceAndProductivity,
    FitnessAndPhysicalVitality,
    TransformationAndSelfDiscovery,
    SocialGrowthAndConnection,
    TimeManagement,
    NutritionAndSupplementation,
    SleepOptimization,
    StressManagement,
    CognitiveEnhancement
}

[Table("biohacks")]
public class Biohack
{
    [System.ComponentModel.DataAnnotations.Schema.Column("id")]
    public int Id { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("title")]
    public string Title { get; set; } = null!;
    [System.ComponentModel.DataAnnotations.Schema.Column("technique")]
    public string? Technique { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("category")]
    public BiohackCategory? Category { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("difficulty")]
    public string? Difficulty { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("timerequired")]
    public string? TimeRequired { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("action")]
    public List<string> Action { get; set; } = new();
    [System.ComponentModel.DataAnnotations.Schema.Column("mechanism")]
    public string? Mechanism { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("researchstudies")]
    public string? ResearchStudies { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("biology")]
    public string? Biology { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("colorgradient")]
    public string? ColorGradient { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("createddate")]
    public DateTime CreatedDate { get; set; }
    [System.ComponentModel.DataAnnotations.Schema.Column("updateddate")]
    public DateTime UpdatedDate { get; set; }

    public ICollection<MotivationBiohack>? MotivationBiohacks { get; set; }
}
