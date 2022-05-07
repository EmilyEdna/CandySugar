using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Entity.Model
{
    public class Candy_YY_LiShi:BasicEntity
    {
        public string Address { get; set; }
        public string CacheAddress { get; set; }
        public string SongName { get; set; }
        public string SongAlbum { get; set; }
        public string SongArtist { get; set; }
        public string SongId { get; set; }
        public int Platform { get; set; }
        public bool IsPlayed { get; set; }
    }
}
