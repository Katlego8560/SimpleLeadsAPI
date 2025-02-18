using Microsoft.EntityFrameworkCore;
using SimpleLeadsAPI.Models;

namespace SimpleLeadsAPI.Services
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Lead> Leads { get; set; } //table that allows us to create a domain model which is saved in our database.

    }
}
