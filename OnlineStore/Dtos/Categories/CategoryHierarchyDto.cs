using System.Text.Json.Serialization;



public class CategoryHierarchyDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryHierarchyDto[] Children { get; set; }
    [JsonIgnore]
    public int? ParentId { get; set; }
}