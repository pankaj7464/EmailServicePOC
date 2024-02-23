using EmailService.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Server.DatabaseConfig
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options):base(options) { }

        /// <summary>
        /// Gets or sets the collection of users.
        /// </summary>
        public DbSet<User> Users => Set<User>();
    }
}
