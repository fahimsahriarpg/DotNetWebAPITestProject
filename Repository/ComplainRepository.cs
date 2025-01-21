using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Context;
using WebApplication8.Interfaces;
using WebApplication8.Model;
using WebApplication8.Model.DTO;

namespace WebApplication8.Repository
{
    public class ComplainRepository : IComplain
    {
        private readonly ApplicationDbContext _context;
        public ComplainRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Complaint> AddComplaintAsync(Complaint complaint)
        { 
            _context.Complaints.Add(complaint); 
            await _context.SaveChangesAsync(); 
            return complaint; 
        }
        public async Task AddComplaintImageAsync(ComplaintImage complaintImage) 
        { 
            _context.ComplaintImages.Add(complaintImage); 
            await _context.SaveChangesAsync(); 
        }

        public async Task<List<ComplaintWithImagesDto>> GetAllComplaintsWithImagesAsync()
        {
            var complaints = await _context.Complaints.Include(c => c.Images).ToListAsync();

            var complaintDtos = complaints.Select(complaint => new ComplaintWithImagesDto
            {
                Id = complaint.Id,
                Name = complaint.Name,
                Detail = complaint.Detail,
                Images = complaint.Images != null ? complaint.Images.Select(image => new ComplaintImageDto
                {
                    Id = image.Id,
                    FileName = image.FileName,
                    ActualFileName = image.ActualFileName,
                    FileSize = image.FileSize,
                    FilePath = image.FilePath,
                    FileType = image.FileType
                }).ToList() : new List<ComplaintImageDto>()
            }).ToList();

            return complaintDtos;
        }
    }
}
