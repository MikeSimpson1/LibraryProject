using Microsoft.EntityFrameworkCore;

namespace LibraryApplication.Data
{
    public class ReviewDataDbContext : DbContext
    {
        public ReviewDataDbContext(DbContextOptions<ReviewDataDbContext> options) : base(options)
        {
        }

        public DbSet<ReviewData> Reviews { get; set; }

    }
}
