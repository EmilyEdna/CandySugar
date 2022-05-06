using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.DTO
{
    public class CandyMHLiShiDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Cover { get; set; }
        public string Title { get; set; }
        public string TagKey { get; set; }
    }
}
