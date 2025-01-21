using static System.Net.Mime.MediaTypeNames;

namespace WebApplication8.Model
{
    public class Complaint
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Detail { get; set; }
        public List<ComplaintImage>? Images { get; set; }
    }
}
