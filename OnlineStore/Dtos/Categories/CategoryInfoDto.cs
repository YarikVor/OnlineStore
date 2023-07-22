

public class CategoryInfoDto
{
    public int Id { get; set; }
    public int? ParentId { get; set; } = null;
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string PhotoUri { get; set; }
    public string Description { get; set; }
}