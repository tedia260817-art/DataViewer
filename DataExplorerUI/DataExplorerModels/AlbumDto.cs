namespace DataExplorerModels
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = "Unknown";
        public string FromCompany { get; set; } = "Unknown";
    }
}
