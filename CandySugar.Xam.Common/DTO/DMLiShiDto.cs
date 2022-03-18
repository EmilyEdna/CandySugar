using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.DTO
{
    public class DMLiShiDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public string AnimeName { get; set; }
        public string CollectionName { get; set; }
        public string PlayURL { get; set; }
        public string Cover { get; set; }
    }
}
