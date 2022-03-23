﻿using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CandySugar.Xam.Common.Entity.Model
{
    public class BZ_LiShi: BasicEntity
    {
        public long Id { get; set; }
        /// <summary>
        /// 长
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// 像素
        /// </summary>
        public string Pixel { get; set; }
        /// <summary>
        /// 预览图
        /// </summary>
        public string Preview { get; set; }
        /// <summary>
        /// 原图
        /// </summary>
        public string OriginalJepg { get; set; }
        /// <summary>
        /// 原图
        /// </summary>
        public string OriginalPng { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSizeJepg { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSizePng { get; set; }
        /// <summary>
        /// 分级
        /// </summary>
        public string Rating { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Created { get; set; }
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
    }
}
