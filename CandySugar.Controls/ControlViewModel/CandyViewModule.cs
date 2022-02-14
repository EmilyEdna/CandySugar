using CandySugar.Core.CandyUtily;
using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CandySugar.Controls.ControlViewModel
{
    public static class CandyViewModule
    {
        public static IContainer Container { get; set; }
        public static IStyletIoCBuilder RegistControlViewModule(this IStyletIoCBuilder builder)
        {
            var Types = Assembly.LoadFrom(Path.Combine(Environment.CurrentDirectory, "CandySugar.Controls.dll"))
                .GetTypes().Where(t => t.BaseType == typeof(Screen) && t.IsClass);

            foreach (var type in Types)
            {
                CandyContainer.Instance.Regiest(type);
            }
            return builder;
        }

    }
}
