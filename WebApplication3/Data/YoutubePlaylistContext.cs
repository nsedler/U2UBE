using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U2UBE.Models;

namespace U2UBE.Data
{
    public class YoutubePlaylistContext : DbContext
    {
        public YoutubePlaylistContext(DbContextOptions<YoutubePlaylistContext> options) : base(options)
        {

        }

        public DbSet<YoutubePlaylist> TPlaylist { get; set; }

    }
}
