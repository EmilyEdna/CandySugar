using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandySugar.Common.WinDTO
{
    public class HAcgOption
    {
        public static List<string> Tags { get; set; }= Tags??new List<string>();
        public static List<string> Brands { get; set; } = Brands ?? new List<string>();
        public static string Type { get; set; }
    }
}
