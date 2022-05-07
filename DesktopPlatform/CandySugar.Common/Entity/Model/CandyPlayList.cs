using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Entity.Model
{
    [SugarTable("CandyPlayList")]
    public class CandyPlayList: BasicEntity
    {
        [SugarColumn(ColumnDataType = "varchar(150)", IsNullable = true)]
        public string SongId { get; set; }
        [SugarColumn(ColumnDataType = "varchar(500)", IsNullable = true)]
        public string Address { get; set; }
        [SugarColumn(ColumnDataType = "varchar(200)", IsNullable = true)]
        public string CacheAddress { get; set; }
        [SugarColumn(ColumnDataType = "varchar(200)", IsNullable = true)]
        public string SongName { get; set; }
        [SugarColumn(ColumnDataType = "varchar(200)", IsNullable = true)]
        public string SongAlbum { get; set; }
        [SugarColumn(ColumnDataType = "varchar(200)", IsNullable = true)]
        public string SongArtist { get; set; }
        [SugarColumn(ColumnDataType = "int", IsNullable = false)]
        public int Platform { get; set; }
    }
}
