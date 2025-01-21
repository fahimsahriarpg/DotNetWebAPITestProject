namespace WebApplication8.Model
{
    public class ComplaintImage
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? ActualFileName { get; set; }
        public string? FileSize { get; set; }
        public string? FilePath { get; set; }
        public string? FileType { get; set; }
        public int ComplaintId { get; set; }
        public Complaint? Complaint { get; set; }
    }
}
