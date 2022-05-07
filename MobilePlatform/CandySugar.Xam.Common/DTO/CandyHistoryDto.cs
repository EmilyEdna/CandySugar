using CandySugar.Xam.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.DTO
{
    public class CandyHistoryDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public string CheckText { get; set; }
        public CheckFuncType CheckType { get; set; }
    }
}
