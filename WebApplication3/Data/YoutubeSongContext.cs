using Microsoft.EntityFrameworkCore;

namespace U2UBE.Models
{
    public class YoutubeSongContext : DbContext
    {
        public YoutubeSongContext(DbContextOptions<YoutubeSongContext> options) : base(options) 
        { 

        }

        public DbSet<YoutubeSong> TSongs { get; set; }

    }
}
