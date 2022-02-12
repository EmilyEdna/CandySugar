using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.DTO
{
    public class CandyMangaHistoryDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public DateTime Time => new DateTime(Span);
        public string Name { get; set; }
        public string Address { get; set; }
        public string Title { get; set; }
        public string TagKey { get; set; }
        public int Index { get; set; }
        public string Chapters { get; set; }
    }
}
