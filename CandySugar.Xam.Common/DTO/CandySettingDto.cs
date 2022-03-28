using System;
using System.Collections.Generic;
using System.Text;

namespace CandySugar.Xam.Common.DTO
{
    public class CandySettingDto
    {
        public Guid PId { get; set; }
        public long Span { get; set; }
        public string ProxyIP { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyPwd { get; set; }
        public string ProxyAccount { get; set; }
        public int CacheTime { get; set; }
        public int WaitSpan { get; set; }
        /// <summary>
        /// 壁纸年龄模式 [0：ALL]---[1：12]---[2：15]---[3：18]
        /// </summary>
        public int AgeModule { get; set; }
    }
}
