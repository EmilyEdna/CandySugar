using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.DTO
{
    public class CandyAnimeHistoryDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public DateTime Time => new DateTime(Span);
        public string PlayURL { get; set; }
        public string AnimeName { get; set; }
        public string CollectName { get; set; }
        public bool PlayMode { get; set; }
        public string PlayModelDes => PlayMode ? "DPlayer" : "VLC";
    }
}
