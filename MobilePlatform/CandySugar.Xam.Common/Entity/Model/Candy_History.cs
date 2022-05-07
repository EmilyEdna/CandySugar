using CandySugar.Xam.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.Entity.Model
{
    /// <summary>
    /// 检索历史
    /// </summary>
    public class Candy_History: BasicEntity
    {
        public string CheckText { get; set; }
        public CheckFuncType CheckType { get; set; }
    }
}
