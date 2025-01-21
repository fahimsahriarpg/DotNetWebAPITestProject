namespace WebApplication8.Model.DTO
{
    public class ComplaintDto
    {
        public string? Name { get; set; }
        public string? Detail { get; set; }
        public List<IFormFile>? File { get; set; }
    }
}
