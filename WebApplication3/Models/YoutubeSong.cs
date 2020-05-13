using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace U2UBE.Models
{
    public class YoutubeSong
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long Length { get; set; }
        public string Url { get; set; }
    }
}
