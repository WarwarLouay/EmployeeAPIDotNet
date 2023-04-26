using EmployeeAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAPI.Data
{
    public class DataContext : DbContext
    {
        internal readonly object Entities;

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> user { get; set; }
        public DbSet<Post> post { get; set; }
    }
}
