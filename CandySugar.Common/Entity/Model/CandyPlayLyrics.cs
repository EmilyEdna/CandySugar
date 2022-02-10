using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Entity.Model
{
    [SugarTable("CandyPlayLyrics")]
    public class CandyPlayLyrics: BasicEntity
    {
        [SugarColumn(ColumnDataType = "varchar(150)", IsNullable = true)]
        public string SongId { get; set; }
        [SugarColumn(ColumnDataType = "int", IsNullable = false)]
        public int Platform { get; set; }
        [SugarColumn(ColumnDataType = "varchar(1000)", IsNullable = true)]
        public string Lyric { get; set; }
    }
}
