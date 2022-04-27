using CandySugar.Common;
using CandySugar.Common.Navigations;
using CandySugar.Controls.ControlViewModel;
using CandySugar.Core.Service;
using CandySugar.Core.ServiceImpl;
using CandySugar.WPF.ViewModels;
using SDKCore;
using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using XExten.Advance.HttpFramework.MultiFactory;
using XExten.Advance.LinqFramework;
using XExten.Advance.StaticFramework;

namespace CandySugar.WPF
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        protected override void OnStart()
        {
            HelpUtilty.DeleteLog();

            //日志
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("Logs/Candy.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //校验版本
            var currentVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var serverVersion = HelpUtilty.GetVersion();
            if (!currentVersion.Equals(serverVersion))
            {
                //升级
                var result = HandyControl.Controls.MessageBox.Info("检测到新版本，即将升级", "提示");
                HelpUtilty.WirteLog("检测到新版本");
                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "CandySugar.Upgrade.exe");
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();//启动
                        process.CloseMainWindow();//通过向进程的主窗口发送关闭消息来关闭拥有用户界面的进程
                        process.Close();//释放与此组件关联的所有资源
                        Environment.Exit(0);
                        Application.Current.Shutdown();
                    }
                    catch (Exception)
                    {
                        Environment.Exit(0);
                        Application.Current.Shutdown();
                    }
                }
            }
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.RegistControlViewModule();
            builder.Bind<IYinYue>().To<YinYue>();
            builder.Bind<IBiZhi>().To<BiZhi>();
            builder.Bind<ILiShi>().To<LiShi>();
            builder.Bind<IAx>().To<Ax>();
            builder.Bind<NavigationController>().And<INavigationController>().To<NavigationController>().InSingletonScope();
        }

        /// <summary>
        /// 初始化系统相关参数配置
        /// </summary>
        protected override void Configure()
        {

            new SqlSugarDbContext().InitCandy();

            BootResource.ReadUserSetting();
            License.UseDisableLicense("ImgDownload");
            base.Configure();
        }

        /// <summary>
        /// 初始化VM
        /// </summary>
        protected override void Launch()
        {
            base.Launch();
        }

        /// <summary>
        /// 加载首页VM
        /// </summary>
        /// <param name="rootViewModel"></param>
        protected override void DisplayRootView(object rootViewModel)
        {
            base.DisplayRootView(rootViewModel);
        }

        /// <summary>
        ///VM加载完毕
        /// </summary>
        protected override void OnLaunch()
        {
            #if !DEBUG
            if (!HelpUtilty.CheckIntegrity())
            {
                var handle = HandyControl.Controls.MessageBox.Error("文件损坏，请重新下载！", "错误");
                HelpUtilty.WirteLog("文件损坏，请重新下载");
                if (handle == MessageBoxResult.OK)
                    Application.Current.Shutdown();
            }
            #endif
            var navigationController = this.Container.Get<NavigationController>();
            navigationController.Delegate = this.RootViewModel;
            CandyViewModule.Container = this.Container;
            base.OnLaunch();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            SyncStatic.DeleteFolder(Path.Combine(Environment.CurrentDirectory, "Lote.exe.WebView2"));
            HelpUtilty.WirteLog("已退出");
            base.OnExit(e);
        }

        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            HelpUtilty.WirteLog("", e.Exception.InnerException ?? e.Exception);
            HandyControl.Controls.MessageBox.Error("服务异常，程序将自动关闭。", "错误");
            GC.Collect();
            Application.Current.Shutdown();
        }
    }
}
