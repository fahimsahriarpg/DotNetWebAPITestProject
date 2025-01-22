using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Context;
using WebApplication8.Interfaces;
using WebApplication8.Model;
using WebApplication8.Model.DTO;

namespace WebApplication8.Repository
{
    public class ComplainRepository : IComplaintRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly DapperDbContext _dapper_context;

        public ComplainRepository(ApplicationDbContext context, DapperDbContext dapper_context)
        {
            _context = context;
            _dapper_context = dapper_context;
        }

        public async Task<Complaint> AddComplaintAsync(Complaint complaint)
        {
            //_context.Complaints.Add(complaint); 
            //await _context.SaveChangesAsync(); 

            // Implementation using Dapper
            using (var connection = _dapper_context.CreateConnection()) 
            { 
                string insertComplaintSql = @" INSERT INTO Complaints (PropertyID, SegmentID, ComplainName, Description) OUTPUT INSERTED.Id VALUES (@PropertyID, @SegmentID, @ComplainName, @Description);"; 
                complaint.Id = await connection.ExecuteScalarAsync<int>(insertComplaintSql, complaint); 
            }
            return complaint; 
        }
        public async Task AddComplaintImageAsync(ComplaintImage complaintImage) 
        {
            //_context.ComplaintImages.Add(complaintImage); 
            //await _context.SaveChangesAsync();

            // Implementation using Dapper
            using (var connection = _dapper_context.CreateConnection()) 
            { 
                string insertImageSql = @" INSERT INTO ComplaintImages (FileName, ActualFileName, FileSize, FilePath, FileType, ComplaintId) VALUES (@FileName, @ActualFileName, @FileSize, @FilePath, @FileType, @ComplaintId);"; 
                await connection.ExecuteAsync(insertImageSql, complaintImage); 
            }
        }

        public async Task<List<ComplaintWithImagesDto>> GetAllComplaintsWithImagesAsync()
        {
            using (var connection = _dapper_context.CreateConnection())
            {
                // Query to get all complaints
                string getAllComplaintsSql = "SELECT * FROM Complaints";
                var complaints = await connection.QueryAsync<ComplaintWithImagesDto>(getAllComplaintsSql);

                // Query to get all images
                string getAllImagesSql = "SELECT * FROM ComplaintImages";
                var images = await connection.QueryAsync<ComplaintImage>(getAllImagesSql);

                // Combine the results
                var complaintDict = complaints.ToDictionary(complaint => complaint.Id, complaint => complaint);

                foreach (var image in images)
                {
                    if (complaintDict.TryGetValue(image.ComplaintId, out var complaint))
                    {
                        complaint.Images ??= new List<ComplaintImageDto>();
                        var complaintImage = new ComplaintImageDto
                        {
                            Id = image.Id,
                            FileName = image.FileName,
                            ActualFileName = image.ActualFileName,
                            FileSize = image.FileSize,
                            FilePath = image.FilePath,
                            FileType = image.FileType
                        };
                        complaint.Images.Add(complaintImage);
                    }
                }

                return complaints.ToList();
            }
            
            //var complaints = await _context.Complaints.Include(c => c.Images).ToListAsync();

            //var complaintDtos = complaints.Select(complaint => new ComplaintWithImagesDto
            //{
            //    Id = complaint.Id,
            //    Name = complaint.Name,
            //    Detail = complaint.Detail,
            //    Images = complaint.Images != null ? complaint.Images.Select(image => new ComplaintImageDto
            //    {
            //        Id = image.Id,
            //        FileName = image.FileName,
            //        ActualFileName = image.ActualFileName,
            //        FileSize = image.FileSize,
            //        FilePath = image.FilePath,
            //        FileType = image.FileType
            //    }).ToList() : new List<ComplaintImageDto>()
            //}).ToList();

            //return complaintDtos;
        }
    }
}
