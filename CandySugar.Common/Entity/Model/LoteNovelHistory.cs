using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Entity.Model
{
    [SugarTable("LoteNovelHistory")]
    public class LoteNovelHistory: BasicEntity
    {
        /// <summary>
        /// 上一页
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string PreviousPage { get; set; }
        /// <summary>
        /// 下一页
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string NextPage { get; set; }
        /// <summary>
        /// 上一章
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string PreviousChapter { get; set; }
        /// <summary>
        ///下一章
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string NextChapter { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(3000)")]
        public string Content { get; set; }
        /// <summary>
        /// 章节
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string ChapterName { get; set; }
        /// <summary>
        /// 书名
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string BookName { get; set; }
    }
}
