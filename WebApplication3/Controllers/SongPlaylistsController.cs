using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using SQLitePCL;
using U2UBE.Data;
using U2UBE.Models;

namespace U2UBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongPlaylistsController : ControllerBase
    {
        private readonly YoutubeSongPlaylistContext _context;
        private readonly YoutubePlaylistContext _playlistContext;
        private readonly YoutubeSongContext _youtubeSongContext;

        public SongPlaylistsController(YoutubeSongPlaylistContext context, YoutubePlaylistContext youtubePlaylistContext, YoutubeSongContext youtubeSongContext)
        {
            _context = context;
            _playlistContext = youtubePlaylistContext;
            _youtubeSongContext = youtubeSongContext;
        }

        // GET: api/SongPlaylists
        [HttpGet]
        public dynamic GetTSongPlaylist()
        {
            var TSongPlaylist = _context.TSongPlaylist.ToList<SongPlaylist>();
            var TPlaylist = _playlistContext.TPlaylist.ToList<YoutubePlaylist>();
            var TSongs = _youtubeSongContext.TSongs.ToList<YoutubeSong>();

            var PlaylistsSongs = from TSP in TSongPlaylist
                       join TP in TPlaylist
                            on TSP.PlaylistId equals TP.Id
                       join TS in TSongs
                           on TSP.SongId equals TS.Id
                       select new
                       {
                           PlayListName = TP.Name,
                           SongName = TS.Title
                       };

            return PlaylistsSongs;
        }
    }
}
