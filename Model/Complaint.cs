using static System.Net.Mime.MediaTypeNames;

namespace WebApplication8.Model
{
    public class Complaint
    {
        public int Id { get; set; }
        public string? PropertyID { get; set; }
        public string? SegmentID { get; set; }
        public string? ComplainName { get; set; }
        public string? Description { get; set; }
        public List<ComplaintImage>? Images { get; set; }
    }
}
