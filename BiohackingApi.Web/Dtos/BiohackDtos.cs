using System.Text.Json;

namespace BiohackingApi.Web.Dtos;

// Biohack DTOs
public class CreateBiohackDto
{
    public string Name { get; set; } = string.Empty;
    public JsonDocument? InfoSections { get; set; }
}

public class UpdateBiohackDto
{
    public string? Name { get; set; }
    public JsonDocument? InfoSections { get; set; }
}

public class ReadBiohackDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public JsonDocument? InfoSections { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
