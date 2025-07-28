namespace BiohackingApi.Web.Dtos;

// Journal DTOs
public class CreateJournalDto
{
    public int UserId { get; set; }
    public int BiohackId { get; set; }
    public string? Notes { get; set; }
    public int? Rating { get; set; }
    public DateTime DateTime { get; set; }
}

public class UpdateJournalDto
{
    public int? UserId { get; set; }
    public int? BiohackId { get; set; }
    public string? Notes { get; set; }
    public int? Rating { get; set; }
    public DateTime? DateTime { get; set; }
}

public class ReadJournalDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFirstName { get; set; } = string.Empty;
    public string UserLastName { get; set; } = string.Empty;
    public int BiohackId { get; set; }
    public string BiohackName { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public int? Rating { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}
