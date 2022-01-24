using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Entity.Model
{
    [SugarTable("LoteAnimeHistory")]
    public class LoteAnimeHistory: BasicEntity
    {
        [SugarColumn(ColumnDataType = "varchar(250)")]
        public string PlayURL { get; set; }
        [SugarColumn(ColumnDataType = "varchar(50)")]
        public string AnimeName { get; set; }
        [SugarColumn(ColumnDataType = "varchar(50)")]
        public string CollectName { get; set; }
        [SugarColumn(ColumnDataType = "bit")]
        public bool PlayMode { get; set; }
    }
}
