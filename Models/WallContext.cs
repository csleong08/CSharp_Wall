using Microsoft.EntityFrameworkCore;

namespace Wall.Models
{
    public class WallContext : DbContext
    {
        public DbSet<Users> users { get; set; } // always make users lowercase
        public DbSet<Comments> comments { get; set; } // always make users lowercase
        public DbSet<Messages> messages { get; set; } // always make users lowercase

        // base() calls the parent class' constructor passing the "options" parameter along
        public WallContext(DbContextOptions<WallContext> options) : base(options) { }
    }
}