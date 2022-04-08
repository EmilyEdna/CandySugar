using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Entity.Model
{
    /// <summary>
    /// 日志收集
    /// </summary>
    public class Candy_Global_Log : BasicEntity
    {
        public string Location { get; set; }
        public string ErrorMsg { get; set; }
        public string ErrorStack { get; set; }
    }
}
