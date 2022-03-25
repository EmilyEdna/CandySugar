using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.DTO
{
    public class CandyLXSLiShiDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public string BookName { get; set; }
        public string ChapterName { get; set; }
        public string ChapeterAddress { get; set; }
        public bool IsBook { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Cover { get; set; }
    }
}
