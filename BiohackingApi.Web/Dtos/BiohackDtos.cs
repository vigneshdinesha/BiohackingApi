using System.Text.Json;
using BiohackingApi.Web.Models;

namespace BiohackingApi.Web.Dtos;

// Biohack DTOs
public class CreateBiohackDto
{
    public string Title { get; set; } = null!;
    public string? Technique { get; set; }
    public BiohackCategory? Category { get; set; }
    public string? Difficulty { get; set; }
    public string? TimeRequired { get; set; }
    public List<string> Action { get; set; } = new();
    public string? Mechanism { get; set; }
    public string? ResearchStudies { get; set; }
    public string? Biology { get; set; }
    public string? ColorGradient { get; set; }
}

public class UpdateBiohackDto
{
    public string? Title { get; set; }
    public string? Technique { get; set; }
    public BiohackCategory? Category { get; set; }
    public string? Difficulty { get; set; }
    public string? TimeRequired { get; set; }
    public List<string>? Action { get; set; }
    public string? Mechanism { get; set; }
    public string? ResearchStudies { get; set; }
    public string? Biology { get; set; }
    public string? ColorGradient { get; set; }
}

public class ReadBiohackDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Technique { get; set; }
    public BiohackCategory? Category { get; set; }
    public string? Difficulty { get; set; }
    public string? TimeRequired { get; set; }
    public List<string> Action { get; set; } = new();
    public string? Mechanism { get; set; }
    public string? ResearchStudies { get; set; }
    public string? Biology { get; set; }
    public string? ColorGradient { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

// Filter DTO for advanced biohack filtering
public class BiohackFilterDto
{
    public BiohackCategory? Category { get; set; }
    public string? Technique { get; set; }
    public string? Difficulty { get; set; }
    public string? TimeRequired { get; set; }
    public string? SearchTerm { get; set; } // For searching in title, mechanism, or biology
}
