using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;

namespace CandySugar.Common
{
    public class HelpUtilty
    {
        public static void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        public static string FileNameFilter(string input)
        {
            string[] Filter = { ":", "\\", "/", "*", "?", "<", ">", "|", "\"" };
            Filter.ForArrayEach<string>(item =>
            {
                if (input.Contains(item))
                {
                    input = input.Replace(item, "_");
                }
            });
            return input;
        }

    }
}
