using Microsoft.AspNetCore.Mvc;
using WebApplication8.Context;
using WebApplication8.Model;
using WebApplication8.Model.DTO;
using WebApplication8.Interfaces;

namespace WebApplication8.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ComplaintsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IComplaintRepository _complaintRepository;

        public ComplaintsController(ApplicationDbContext context, IComplaintRepository complaintRepository)
        {
            _context = context;
            _complaintRepository = complaintRepository;
        }

        
        [HttpGet, Route("GetComplains")]
        public async Task<ActionResult<List<ComplaintWithImagesDto>>> GetAllComplaintsWithImages()
        {
            var complaintDtos = await _complaintRepository.GetAllComplaintsWithImagesAsync();
            return Ok(complaintDtos);
        }

        [HttpPost, Route("SaveComplain")]  
        public async Task<IActionResult> SaveComplainAsync([FromForm] ComplaintDto complaintDto)
        {
            if (complaintDto == null)
            {
                return BadRequest("Complaint data is required.");
            }

            // Create the complaint entity
            var complaint = new Complaint
            {
                PropertyID = complaintDto.PropertyID,
                SegmentID = complaintDto.SegmentID,
                ComplainName = complaintDto.ComplainName ?? "",
                Description = complaintDto.Description ?? "",
                Images = new List<ComplaintImage>()
            };
            // Save the complaint entity to the database
            await _complaintRepository.AddComplaintAsync(complaint);

            if (complaintDto.File != null && complaintDto.File.Count > 0)
            {
                // Check if the folder exists, if not create it
                var folderPath = "D:\\RMSImages\\Complain\\Images";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (var formFile in complaintDto.File)
                {
                    if (formFile.Length > 0)
                    {
                        // Get the file extension
                        var fileExtension = Path.GetExtension(formFile.FileName);

                        // Generate a unique file name with the extension
                        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                        // Save the file to the local computer's D drive
                        var filePath = Path.Combine(folderPath, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }

                        // Convert file size to KB and MB
                        double fileSizeInKB = formFile.Length / 1024.0;
                        double fileSizeInMB = fileSizeInKB / 1024.0;
                        string fileSizeWithUnit;
                        if (fileSizeInMB >= 1)
                        {
                            fileSizeWithUnit = fileSizeInMB.ToString("F2") + " MB";
                        }
                        else
                        {
                            fileSizeWithUnit = fileSizeInKB.ToString("F2") + " KB";
                        }
                        // Create the complaint image entity
                        var complaintImage = new ComplaintImage
                        {
                            FileName = uniqueFileName,
                            ActualFileName = formFile.FileName,
                            FileSize = fileSizeWithUnit,
                            FilePath = filePath,
                            FileType = fileExtension,
                            ComplaintId = complaint.Id
                        };

                        await _complaintRepository.AddComplaintImageAsync(complaintImage);
                    }
                }
            }

            return Ok("Complaint submitted successfully.");
        }
    }
}
