using iTaaS.ConvertLogService.Models;
using Microsoft.EntityFrameworkCore;

namespace iTaaS.ConvertLogService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :  base(options) { }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Destination> Destinations { get; set; }
    }
}
