namespace DataExplorerModels;

public class Post
{
    public int Id { get; set; }
   public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsExpanded { get; set; } = false;
}
