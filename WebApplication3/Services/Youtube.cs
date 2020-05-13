using MediaToolkit;
using MediaToolkit.Model;
using System.IO;
using YoutubeExplode;
using System.Threading.Tasks;
using YoutubeExplode.Videos;
using YoutubeExplode.Converter;
using System.Linq;
using YoutubeExplode.Playlists;
using System;

namespace U2UBE.Services
{
    public class Youtube
    {
        private string Url { get; set; }
        private string Path { get; set; }

        private YoutubeClient _YoutubeClient = new YoutubeClient();
        private IYoutubeConverter _YoutubeConverter = new YoutubeConverter();
        private Video _Video;
        private Playlist _Playlist;

        public Youtube(string Url, string Path)
        {
            this.Url = Url;
            this.Path = Path;
        }

        // This gets the Video object for our youtube video
        public async Task<Tuple<Video, Playlist>> GetYoutubeVideoAsync()
        {

            Tuple<Video, Playlist> ReturnTuple;
             
            if (this.Url.Contains("&list=") || this.Url.Contains("playlist?list"))
            {
                _Playlist = await _YoutubeClient.Playlists.GetAsync(this.Url);
            }
            else if (this.Url.Contains("www.youtube.com/watch?v="))
            {
                _Video = await _YoutubeClient.Videos.GetAsync(this.Url);
            }
            else
            {
                var Search = await _YoutubeClient.Search.GetVideosAsync(this.Url);
                _Video = await _YoutubeClient.Videos.GetAsync(Search.First().Url);
            }

            ReturnTuple = new Tuple<Video, Playlist>(_Video, _Playlist);
            return ReturnTuple;
        }

        // This downloads the video and return wheather or not the video exists
        public async Task DownloadYoutubeVideoAsync()
        {

            Tuple<Video, Playlist> ReturnTuple = await GetYoutubeVideoAsync();

            if(ReturnTuple.Item1 != null)
            {
                await _YoutubeConverter.DownloadVideoAsync(_Video.Id, this.Path + _Video.Title + ".mp3");
            }
            else
            {
                await foreach(var video in _YoutubeClient.Playlists.GetVideosAsync(_Playlist.Id))
                {
                    await _YoutubeConverter.DownloadVideoAsync(video.Id, this.Path + video.Title + ".mp3");
                }
            }
            
        }
    }
}
