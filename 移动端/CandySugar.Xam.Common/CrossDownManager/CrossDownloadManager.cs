using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CandySugar.Xam.Common.CrossDownManager
{
    public class CrossDownloadManager
    {
        private static Lazy<IDownloadManager> Implementation = new Lazy<IDownloadManager>(() => CreateDownloadManager(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
        /// <summary>
        /// The platform-implementation
        /// </summary>
        public static IDownloadManager Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
                }
                return ret;
            }
        }

        private static IDownloadManager CreateDownloadManager()
        {
            var type = Assembly.Load("CandySugar.Android.dll").GetTypes().Where(t => t.GetInterface("IDownloadManager") == typeof(IDownloadManager)).FirstOrDefault();
            return (IDownloadManager)Activator.CreateInstance(type);
        }
    }
}
