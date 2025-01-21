using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Model;

namespace WebApplication8.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<ComplaintImage> ComplaintImages { get; set; }
    }
}
