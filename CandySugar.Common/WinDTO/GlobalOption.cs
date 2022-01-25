using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Common.WinDTO
{
    public class GlobalOption
    {
        public string ProxyIP { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyAccount { get; set; }
        public string ProxyPwd { get; set; }
        public string NovelAccount { get; set; }
        public string NovelPwd { get; set; }
        public bool DefaultNovel { get; set; }
        public int CacheTime { get; set; }
        public int PlayBox { get; set; }
    }
}
