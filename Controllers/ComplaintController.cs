using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using WebApplication8.Context;
using WebApplication8.Model;
using WebApplication8.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace WebApplication8.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ComplaintsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getAllComplaintsWithImages")]
        public async Task<ActionResult<List<ComplaintWithImagesDto>>> GetAllComplaintsWithImages()
        {
            var complaints = await _context.Complaints.Include(c => c.Images).ToListAsync();

            var complaintDtos = complaints.Select(complaint => new ComplaintWithImagesDto
            {
                Id = complaint.Id,
                Name = complaint.Name,
                Detail = complaint.Detail,
                Images = complaint.Images.Select(image => new ComplaintImageDto
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    ActualFileName = image.ActualFileName,
                    FileSize = image.FileSize,
                    FilePath = image.FilePath,
                    FileType = image.FileType
                }).ToList()
            }).ToList();

            return Ok(complaintDtos);
        }

        [HttpPost]
        [Route("addComplaint")]
        public async Task<IActionResult> SubmitComplaintWithImages([FromForm] ComplaintDto complaintDto)
        {
            if (complaintDto == null)
            {
                return BadRequest("Complaint data is required.");
            }

            // Create the complaint entity
            var complaint = new Complaint
            {
                Name = complaintDto.Name,
                Detail = complaintDto.Detail,
                Images = new List<ComplaintImage>()
            };

            // Save the complaint entity to the database
            _context.Complaints.Add(complaint);
            await _context.SaveChangesAsync();

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
                        } else { 
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

                        // Save the complaint image entity to the database
                        _context.ComplaintImages.Add(complaintImage);
                        complaint.Images.Add(complaintImage);
                    }
                }

                await _context.SaveChangesAsync();
            }

            return Ok("Complaint submitted successfully.");
        }
    }
}
