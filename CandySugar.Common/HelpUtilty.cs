using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

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
        public static bool CheckIntegrity()
        {
            string[] file = { "CandySugar.dll", "CandySugar.exe", "CandySugar.Common.dll", "CandySugar.Controls.dll", "CandySugar.Core.dll", "CandySugar.Resource.dll", "CandySugar.Upgrade.exe", "CandySugar.Upgrade.dll" };

            var files = Directory.GetFiles(Environment.CurrentDirectory)
                 .Select(t => t.Split("\\").LastOrDefault())
                 .Where(t => t.Contains("CandySugar"))
                 .Where(t => Path.GetExtension(t).Contains("exe") || Path.GetExtension(t).Contains("dll"))
                 .Select(t => t).ToList();

            foreach (var item in files)
            {
                if (file.Contains(item))
                    continue;
                else
                    return false;
            }
            return true;
        }
        public static void WirteLog(string Handle, Exception Ex = null)
        {
            if (Ex != null)
                Log.Error(Ex, "");
            else
                Log.Logger.Information($"CandySugar：【{Handle}】，时间：【{DateTime.Now.ToFmtDate(3, "yyyy年MM月dd日 HH时mm分ss秒")}】");
        }
        public static void DeleteLog() 
        {
            SyncStatic.DeleteFolder(Path.Combine(Environment.CurrentDirectory, "Logs"));
        }
    }
}
