namespace BiohackingApi.Web.Dtos;

// User DTOs
public class CreateUserDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public string? ExternalId { get; set; }
    public string? SubId { get; set; }
    public int? MotivationId { get; set; }
}

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Provider { get; set; }
    public string? ExternalId { get; set; }
    public string? SubId { get; set; }
    public int? MotivationId { get; set; }
}

public class ReadUserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public string? ExternalId { get; set; }
    public string? SubId { get; set; }
    public int? MotivationId { get; set; }
    public string? MotivationName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

// User-Motivation Link DTOs
public class LinkUserMotivationDto
{
    public int UserId { get; set; }
    public int MotivationId { get; set; }
}

public class UnlinkUserMotivationDto
{
    public int UserId { get; set; }
}
