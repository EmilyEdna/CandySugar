using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.Entity.Model
{
    [SugarTable("CandyFavorite")]
    public class CandyFavorite: BasicEntity
    {
        [SugarColumn(ColumnDataType = "bigint")]
        public long Id { get; set; }
        /// <summary>
        /// 长
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Width { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        [SugarColumn(ColumnDataType = "int")]
        public int Height { get; set; }
        /// <summary>
        /// 像素
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(50)")]
        public string Pixel { get; set; }
        /// <summary>
        /// 预览图
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string Preview { get; set; }
        /// <summary>
        /// 原图
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string OriginalJepg { get; set; }
        /// <summary>
        /// 原图
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(255)")]
        public string OriginalPng { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string FileSizeJepg { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string FileSizePng { get; set; }
        /// <summary>
        /// 分级
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(20)")]
        public string Rating { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(800)")]
        public string Label { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<string> Labels
        {
            get
            {
                return Label.Split("|").ToList();
            }
            set
            {
                Label = string.Join("|", value);
            }
        }
        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(ColumnDataType = "datetime")]
        public DateTime Created { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(80)")]
        public string Author { get; set; }
    }
}
