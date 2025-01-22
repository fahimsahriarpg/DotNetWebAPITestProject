namespace WebApplication8.Model.DTO
{
    public class ComplaintWithImagesDto
    {
        public int Id { get; set; }
        public string? PropertyID { get; set; }
        public string? SegmentID { get; set; }
        public string? ComplainName { get; set; }
        public string? Description { get; set; }
        public List<ComplaintImageDto>? Images { get; set; }
    }
}
