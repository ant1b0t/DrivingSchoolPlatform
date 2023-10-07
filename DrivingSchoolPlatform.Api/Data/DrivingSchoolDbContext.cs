using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DrivingSchoolPlatform.Api.Data
{
    public class DrivingSchoolDbContext : IdentityDbContext<IdentityUser>
    {

        public DrivingSchoolDbContext(DbContextOptions<DrivingSchoolDbContext> options) : base(options)
        {
        }
    }
}
