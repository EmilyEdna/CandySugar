using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Entity.Model
{
    public class Candy_LXS_LiShi: BasicEntity
    {
        public string BookName { get; set; }
        public string ChapterName { get; set; }
        public string ChapeterAddress { get; set; }
        public bool IsBook { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Cover { get; set; }
    }
}
