namespace WebApplication8.Model.DTO
{
    public class ComplaintDto
    {
        public string? PropertyID { get; set; }
        public string? SegmentID { get; set; }
        public string? ComplainName { get; set; }
        public string? Description{ get; set; }
        public List<IFormFile>? File { get; set; }
    }
}
