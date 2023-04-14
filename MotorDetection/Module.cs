using Caliburn.Micro;
using Gemini.Framework.Services;
using Gemini.Framework;
using Gemini.Modules.Output;
using Gemini.Modules.Settings.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Gemini.Modules.Settings.Views;

namespace MotorDetection
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        [Import] private IOutput _output;
        //[Import] private IResourceManager _resourceManager;

        public override void Initialize()
        {

            Shell.ShowFloatingWindowsInTaskbar = true;
            Shell.ToolBars.Visible = true;

            //MainWindow.Icon = _resourceManager.GetBitmap("Resources/Images/sglogo-32.jpg", Assembly.GetExecutingAssembly().GetName().Name);

            //MainWindow.WindowState = WindowState.Maximized;

            MainWindow.Title = "MotorDetection";

            MainWindow.Width = 1200;
            MainWindow.Height = 900;

            Shell.StatusBar.AddItem($"欢迎使用 MotorDetection 软件", new GridLength(1, GridUnitType.Star));
            Shell.StatusBar.AddItem("", new GridLength(200));
            Shell.StatusBar.AddItem("", new GridLength(200));
            Shell.StatusBar.AddItem("", new GridLength(200));

            //Shell.StatusBar.AddItem("Ln 44", new GridLength(100));
            //Shell.StatusBar.AddItem("Col 79", new GridLength(100));

            _output.DisplayName = "日志";
            _output.AppendLine("软件启动成功");

            IoC.Get<SettingsViewModel>().ViewAttached += (s, e) =>
            {
                ((SettingsView)e.View).Width = 980;
            };

        }
    }
}
