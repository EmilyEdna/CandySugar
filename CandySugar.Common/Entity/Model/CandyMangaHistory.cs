using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Entity.Model
{
    [SugarTable("CandyMangaHistory")]
    public class CandyMangaHistory: BasicEntity
    {
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string Name { get; set; }
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string Address { get; set; }
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string Title { get; set; }
        [SugarColumn(ColumnDataType = "varchar(80)")]
        public string TagKey { get; set; }
        [SugarColumn(ColumnDataType = "text")]
        public string Chapters { get; set; }
        public int Index { get; set; }
    }
}
