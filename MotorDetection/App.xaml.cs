using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MotorDetection
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //首先注册开始和退出事件
            Startup += App_Startup;
            Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
            //UI线程未捕获异常处理事件
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
        }

        private void App_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                if (e.Exception is COMException comException && comException.ErrorCode == -2147221040)
                {
                    e.Handled = true;
                    return;
                }

                e.Handled = true; //把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出     


                //QuickSV.Properties.Resources.
                MessageBox.Show(
                    $"捕获线程内未处理异常：{e.Exception.Message}\n{e.Exception.InnerException}");
            }
            catch (Exception ex)
            {
                //此时程序出现严重异常，将强制结束退出
                MessageBox.Show("程序发生致命错误，将终止，请联系运营商！");
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var sbEx = new StringBuilder();
            if (e.IsTerminating)
                sbEx.Append($"程序发生致命错误，将终止，请联系运营商！\n");

            sbEx.Append("捕获未处理异常：");
            if (e.ExceptionObject is Exception)
                sbEx.Append(((Exception)e.ExceptionObject).Message);
            else
                sbEx.Append(e.ExceptionObject);

            MessageBox.Show(sbEx.ToString());
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            //task线程内未处理捕获

            MessageBox.Show(
                $"捕获线程内未处理异常：{args.Exception.Message}\n{args.Exception.InnerExceptions.Select(e => e.Message).Aggregate((p, n) => p + "\n" + n)}");

            args.SetObserved(); //设置该异常已察觉（这样处理后就不会引起程序崩溃）
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            path = Path.Combine(path, IntPtr.Size == 8 ? "x64" : "x86");
            var envPath = Environment.GetEnvironmentVariable("path");
            envPath = $"{path};{envPath}";
            Environment.SetEnvironmentVariable("path", envPath);

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            Environment.Exit(0);
        }
    }
}
