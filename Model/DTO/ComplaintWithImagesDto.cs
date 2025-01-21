namespace WebApplication8.Model.DTO
{
    public class ComplaintWithImagesDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Detail { get; set; }
        public List<ComplaintImageDto>? Images { get; set; }
    }
}
