using Microsoft.AspNetCore.Mvc;
using WebApplication8.Model;
using WebApplication8.Model.DTO;

namespace WebApplication8.Interfaces
{
    public interface IComplain
    {
        Task<Complaint> AddComplaintAsync(Complaint complaint); 
        Task AddComplaintImageAsync(ComplaintImage complaintImage);
        Task<List<ComplaintWithImagesDto>> GetAllComplaintsWithImagesAsync();
    }
}
