namespace OnlineStore.Controllers;

public class BlogInfoDto
{
    public int Id { get; set; }
    public DateTimeOffset TimeWriting { get; set; }
    public string PhotoUri { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ContentUri { get; set; } = null!;
}