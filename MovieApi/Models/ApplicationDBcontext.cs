using Microsoft.EntityFrameworkCore;

namespace MovieApi.Models
{
    public class ApplicationDBcontext:DbContext
    {
        public ApplicationDBcontext(DbContextOptions<ApplicationDBcontext> options):base(options)
        {
        }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
    }
}
