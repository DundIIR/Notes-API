using Microsoft.EntityFrameworkCore;
using Notes_API.Models;

namespace Notes_API.DB
{
    public class NotesDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public NotesDbContext(IConfiguration config)
        {
            _config = config;
        }

        public DbSet<Note> Notes => Set<Note>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_config.GetConnectionString("NotesDataBase"));
        }
    }
}
