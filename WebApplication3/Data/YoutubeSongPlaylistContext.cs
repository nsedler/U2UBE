using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U2UBE.Models;

namespace U2UBE.Data
{
    public class YoutubeSongPlaylistContext : DbContext
    {
        public YoutubeSongPlaylistContext(DbContextOptions<YoutubeSongPlaylistContext> options) : base(options)
        {

        }

        public DbSet<SongPlaylist> TSongPlaylist { get; set; }

    }
}
