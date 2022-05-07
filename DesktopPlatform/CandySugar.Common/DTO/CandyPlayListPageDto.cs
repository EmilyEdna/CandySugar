using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.DTO
{
    public class CandyPlayListPageDto
    {
        public int Count { get; set; }
        public int Total { get; set; }
        public List<CandyPlayListDto> Result { get; set; }
    }
}
