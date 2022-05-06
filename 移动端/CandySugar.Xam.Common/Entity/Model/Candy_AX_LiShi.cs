using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Entity.Model
{
    public class Candy_AX_LiShi:BasicEntity
    {
        public bool IsFavorite { get; set; }
        public int VId { get; set; }
        public string Title { get; set; }
        public string KeyWord { get; set; }
        public string Channel { get; set; }
        public string Duration { get; set; }
        public string AddTime { get; set; }
        public int Views { get; set; }
        public string Preview { get; set; }
        public string Play { get; set; }
    }
}
