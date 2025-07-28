namespace BiohackingApi.Web.Dtos;

// Motivation DTOs
public class CreateMotivationDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateMotivationDto
{
    public string? Name { get; set; }
}

public class ReadMotivationDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
