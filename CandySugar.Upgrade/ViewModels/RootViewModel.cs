using HandyControl.Controls;
using Stylet;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using XExten.Advance.StaticFramework;

namespace CandySugar.Upgrade.ViewModels
{
    public class RootViewModel : Conductor<IScreen>
    {
        private readonly string URL = "https://ghproxy.com/https://github.com/EmilyEdna/KuRuMi/releases/download/1.0/UpGrade.zip";

        public RootViewModel()
        {
            this.Grade = 0d;
        }

        private double _Grade;
        public double Grade
        {
            get { return _Grade; }
            set { SetAndNotify(ref _Grade, value); }
        }
        public void Upgrade()
        {
            var dir = SyncStatic.CreateDir(Path.Combine(Environment.CurrentDirectory, "UpdateFile"));
            var fn = Path.Combine(dir, "File.zip");
            SyncStatic.DeleteFile(fn);
            try
            {
                WebClient client = new WebClient();
                client.DownloadDataAsync(new Uri(URL));
                client.DownloadProgressChanged += (c, e) =>
                {
                    Grade = double.Parse((e.BytesReceived * 100.00 / e.TotalBytesToReceive).ToString("F2"));
                };
                client.DownloadDataCompleted += (c, e) =>
                {
                    SyncStatic.WriteFile(e.Result, fn);
                    ExtractZip(fn);
                };
            }
            catch (Exception)
            {
                var result = MessageBox.Show("已是最新版本!", "通知", System.Windows.MessageBoxButton.OK);
                if (result == System.Windows.MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }
            }
        }
        private void ExtractZip(string fn)
        {
            ZipFile.ExtractToDirectory(fn, Environment.CurrentDirectory, true);
            OpenExe();
        }
        private void OpenExe()
        {
            Process process = new Process();
            process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "CandySugar.exe";
            process.StartInfo.CreateNoWindow = true;
            process.Start();//启动
            process.CloseMainWindow();//通过向进程的主窗口发送关闭消息来关闭拥有用户界面的进程
            process.Close();//释放与此组件关联的所有资源
            Environment.Exit(0);
        }
    }
}
