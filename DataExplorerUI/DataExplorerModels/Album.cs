namespace DataExplorerModels
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public int UserId { get; set; }
        public User? Creator { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public Company? Company { get; set; }
    }

    public class Company
    {
        public string Name { get; set; } = string.Empty;
    }
}
