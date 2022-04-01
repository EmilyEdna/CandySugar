using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.DTO
{
    public class CandyYYLiShiDto
    {
        public Guid PId { get; set; }
        public string Address { get; set; }
        public string CacheAddress { get; set; }
        public string SongName { get; set; }
        public string SongAlbum { get; set; }
        public string SongArtist { get; set; }
        public string SongId { get; set; }
        public long Span { get; set; }
        public int Platform { get; set; }
    }
}
