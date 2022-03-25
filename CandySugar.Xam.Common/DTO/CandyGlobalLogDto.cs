using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.DTO
{
    public class CandyGlobalLogDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public string ErrorMsg { get; set; }
        public string ErrorStack { get; set; }
    }
}
