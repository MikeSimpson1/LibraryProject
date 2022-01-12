using Microsoft.EntityFrameworkCore;

namespace LibraryApplication.Data
{
    public class BookDataDbContext : DbContext
    {
        public BookDataDbContext(DbContextOptions<BookDataDbContext> options) : base(options)
        {
        }

        public DbSet<BookData> Books { get; set; }

    }
}
