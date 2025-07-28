namespace BiohackingApi.Web.Dtos;

// MotivationBiohack DTOs
public class CreateMotivationBiohackDto
{
    public int MotivationId { get; set; }
    public int BiohackId { get; set; }
}

public class ReadMotivationBiohackDto
{
    public int MotivationId { get; set; }
    public string MotivationName { get; set; } = string.Empty;
    public int BiohackId { get; set; }
    public string BiohackName { get; set; } = string.Empty;
}

public class LinkMotivationBiohackDto
{
    public int MotivationId { get; set; }
    public int BiohackId { get; set; }
}
