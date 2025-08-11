namespace BiohackingApi.Web.Dtos;

// Motivation DTOs
public class CreateMotivationDto
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
}

public class UpdateMotivationDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public class ReadMotivationDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
