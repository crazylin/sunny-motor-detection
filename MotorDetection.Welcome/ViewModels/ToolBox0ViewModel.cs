using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Modules.OperateToolBox;

namespace MotorDetection.Welcome.ViewModels
{
    [OperateToolboxItem(
        documentType: typeof(WelcomeViewModel),
        name: nameof(MotorDetection.Welcome.Properties.Resources.test),
        category: nameof(MotorDetection.Welcome.Properties.Resources.test),
        order: 0,
        resourceType: typeof(MotorDetection.Welcome.Properties.Resources))]
    [Export(typeof(ToolBox0ViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ToolBox0ViewModel : ViewAware
    {
        private string _text;

        public string Text
        {
            set => Set(ref _text, value);
            get => _text;
        }
    }
}
