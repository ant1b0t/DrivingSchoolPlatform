using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolPlatform.Api.Data
{
    public class DrivingSchoolDbContext : DbContext
    {
        public DbSet<UserDb> Users { get; set; }

        public DrivingSchoolDbContext(DbContextOptions<DrivingSchoolDbContext> options) : base(options)
        {
        }
    }
}
