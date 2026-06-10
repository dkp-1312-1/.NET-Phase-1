using Microsoft.EntityFrameworkCore;
using TraineeManagement1.Models;
namespace TraineeManagement1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }
        public DbSet<Trainee> Trainees {get;set;}
        
    }
}