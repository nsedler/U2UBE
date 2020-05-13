using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using U2UBE.Models;
using U2UBE.Services;
using YoutubeExplode.Videos;
using YoutubeExplode.Playlists;
using YoutubeExplode;
using U2UBE.Data;

namespace U2UBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubeSongsController : ControllerBase
    {
        private readonly YoutubeSongContext _context;
        private readonly YoutubePlaylistContext _playlistContext;
        private readonly YoutubeSongPlaylistContext _songPlaylistContext;

        private Youtube _youtube;
        private YoutubeClient _YoutubeClient = new YoutubeClient();

        public YoutubeSongsController(YoutubeSongContext context, YoutubePlaylistContext youtubePlaylistContext, YoutubeSongPlaylistContext youtubeSongPlaylistContext)
        {
            _context = context;
            _playlistContext = youtubePlaylistContext;
            _songPlaylistContext = youtubeSongPlaylistContext;
        }

        // GET: api/YoutubeSongs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<YoutubeSong>>> GetYoutubeSongs()
        {

            return await _context.TSongs.ToListAsync();
        }

        // GET: api/YoutubeSongs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<YoutubeSong>> GetYoutubeSong(int id)
        {
            var youtubeSong = await _context.TSongs.FindAsync(id);

            if (youtubeSong == null)
            {
                return NotFound();
            }

            return youtubeSong;
        }

        // PUT: api/YoutubeSongs/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutYoutubeSong(int id, YoutubeSong youtubeSong)
        {
            if (id != youtubeSong.Id)
            {
                return BadRequest();
            }

            _context.Entry(youtubeSong).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YoutubeSongExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/YoutubeSongs
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<dynamic>> PostYoutubeSong(YoutubeLink youtubeLink)
        {
            var SongCount = _context.TSongs.ToList<YoutubeSong>().Count;
            var PlaylistCount = _playlistContext.TPlaylist.ToList<YoutubePlaylist>().Count;
            var SongPlaylistCount = _songPlaylistContext.TSongPlaylist.ToList<SongPlaylist>().Count;

            // This gets the YoutubeVideo object and downloads the mp3
            _youtube = new Youtube(youtubeLink.Url, @"C:\Users\nsedler\source\repos\WebApplication3\WebApplication3\Files\");
            Tuple<Video, Playlist> VideoPlaylist = await _youtube.GetYoutubeVideoAsync();
            await _youtube.DownloadYoutubeVideoAsync();

            //Sets the Songs fields
            YoutubeSong _song = new YoutubeSong();

            if (VideoPlaylist.Item1 != null)
            {
                _song = new YoutubeSong();
                var video = VideoPlaylist.Item1;
                _song.Title = video.Title;
                _song.Length = video.Duration.Ticks;
                _song.Url = $"https://localhost:44370/StaticSongs/{video.Title}.mp3";
                _song.Id = SongCount + 1;

                _context.TSongs.Add(_song);
                await _context.SaveChangesAsync();
            }
            else
            {
                YoutubePlaylist YoutubePlaylist = new YoutubePlaylist();
                var Playlist = await _YoutubeClient.Playlists.GetAsync(VideoPlaylist.Item2.Id);

                YoutubePlaylist.Name = Playlist.Title;
                YoutubePlaylist.Id = PlaylistCount + 1;

                _playlistContext.TPlaylist.Add(YoutubePlaylist);
                await _playlistContext.SaveChangesAsync();



                await foreach (var video in _YoutubeClient.Playlists.GetVideosAsync(VideoPlaylist.Item2.Id))
                {
                    _song = new YoutubeSong();
                    _song.Title = video.Title;
                    _song.Length = video.Duration.Ticks;
                    _song.Url = $"https://localhost:44370/StaticSongs/{video.Title}.mp3";
                    _song.Id = SongCount + 1;

                    _context.TSongs.Add(_song);
                    await _context.SaveChangesAsync();

                    SongPlaylist SongPlaylist = new SongPlaylist();
                    SongPlaylist.Id = SongPlaylistCount + 1;
                    SongPlaylist.SongId = _song.Id;
                    SongPlaylist.PlaylistId = YoutubePlaylist.Id;

                    _songPlaylistContext.TSongPlaylist.Add(SongPlaylist);
                    await _songPlaylistContext.SaveChangesAsync();

                    SongPlaylistCount++;
                    SongCount++;
                }

                return CreatedAtAction("GetYoutubeSong", new { id = YoutubePlaylist.Id }, YoutubePlaylist);
            }

            return CreatedAtAction("GetYoutubeSong", new { id = _song.Id }, _song);
        }

        // DELETE: api/YoutubeSongs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<YoutubeSong>> DeleteYoutubeSong(int id)
        {
            var youtubeSong = await _context.TSongs.FindAsync(id);
            if (youtubeSong == null)
            {
                return NotFound();
            }

            _context.TSongs.Remove(youtubeSong);
            await _context.SaveChangesAsync();

            return youtubeSong;
        }

        private bool YoutubeSongExists(int id)
        {
            return _context.TSongs.Any(e => e.Id == id);
        }
    }
}