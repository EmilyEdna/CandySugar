using CandySugar.Controls.Commands;
using CandySugar.Core.CandyUtily;
using StyletIoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.StaticFramework;

namespace CandySugar.Controls.ControlViewModel
{
    public static class CandyViewModule
    {
        public static IStyletIoCBuilder RegistControlViewModule(this IStyletIoCBuilder builder)
        {
            SyncStatic.Assembly("CandySugar.Controls")
                .SelectMany(x => x.ExportedTypes.Where(t => t.BaseType == typeof(CandyCommand) && t.IsClass))
                .ToList().ForEach(item => {
                CandyContainer.Instance.Regiest(item);
            });
            return builder;
        }

    }
}
