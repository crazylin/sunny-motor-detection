using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using MotorDetection.Welcome.Commands;

namespace MotorDetection.Welcome.ViewModels
{
    [Export(typeof(WelcomeViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public sealed class WelcomeViewModel : Document
    {

        private ToolBox0ViewModel toolBox0;
        private string _text;
        private bool _isTest;

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public WelcomeViewModel()
        {
            DisplayName = "欢迎页";
            Text = "欢迎使用软件";
            CanTest = true;
        }

        public bool CanTest
        {
            set => Set(ref _isTest, value);
            get => _isTest;
        }

        public void Test()
        {
            //IoC.Get<IShell>().BusyIndicator.ShowPercentage
            //IoC.Get<IWindowManager>().ShowWindowAsync(this,)

            IoC.Get<ToolBox0ViewModel>().Text = DateTime.Now.ToString();
        }
    }
}
