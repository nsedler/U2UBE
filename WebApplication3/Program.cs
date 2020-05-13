using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using U2UBE.Models;
using U2UBE.Services;
using YoutubeExplode;
using YoutubeExplode.Videos;

namespace U2UBE
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            

            YoutubeClient _YoutubeClient = new YoutubeClient();
            var test = await _YoutubeClient.Search.GetVideosAsync("Still Into You - Paramore ('40s Swing Cover) ft. Maris");
            foreach(Video v in test)
            {
                Console.WriteLine(v.ToString());
            }
            //Youtube U2ube = new Youtube("https://www.youtube.com/watch?v=QBK6xymmKHM", @"C:\Users\nsedler\source\repos\WebApplication3\WebApplication3\Files\");
            //Video Video = await U2ube.GetYoutubeVideoAsync();

            //Console.WriteLine(Video);
            //await U2ube.DownloadYoutubeVideoAsync();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
