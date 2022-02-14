using CandySugar.Upgrade.ViewModels;
using Stylet;
using StyletIoC;
using System;
using System.Windows;
using System.Windows.Threading;

namespace CandySugar.Upgrade
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        /// <summary>
        /// 程序启动
        /// </summary>
        protected override void OnStart()
        {
            //校验版本
        }

        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {

        }

        /// <summary>
        /// 初始化系统相关参数配置
        /// </summary>
        protected override void Configure()
        {
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
            base.OnLaunch();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }

        /// <summary>
        /// 全局异常捕获
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnhandledException(DispatcherUnhandledExceptionEventArgs e)
        {
            HandyControl.Controls.MessageBox.Info("已是最新版本!", "通知");
            GC.Collect();
            Application.Current.Shutdown();
        }
    }
}
