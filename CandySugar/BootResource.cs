using CandySugar.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.Advance.LinqFramework;


namespace CandySugar
{
    public class BootResource
    {
        /// <summary>
        /// 设置全局用户配置的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        public static void SetSettingPropertyValue<T>(T param)
        {
            param.GetType().GetProperties().ForEnumerEach(t =>
            {
                Soft.Default.GetType().GetProperty(t.Name).SetValue(Soft.Default, t.GetValue(param));
            });
        }
    }
}
