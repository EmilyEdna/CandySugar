using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Common.Entity.Model
{
    [SugarTable("CandyGalCover")]
    public class CandyGalCover : BasicEntity
    {
        [SugarColumn(Length = 5000)]
        public string CoverBase64 { get; set; }
        [SugarColumn(Length = 500)]
        public string Cover { get; set; }
    }
}
