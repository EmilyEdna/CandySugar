using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Common.DTO
{
    public class CandyAxFavoritePageDto
    {
        public int Total { get; set; }
        public List<CandyAxFavoriteDto> Result { get; set; }
    }
}
