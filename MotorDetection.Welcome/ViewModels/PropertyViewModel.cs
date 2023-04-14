using Gemini.Modules.OperateToolBox;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Modules.PropertyToolBox;

namespace MotorDetection.Welcome.ViewModels
{
    [PropertyToolboxItem(
        documentType: typeof(WelcomeViewModel),
        name: "欢迎界面谁说谁",
        category: "属性1",
        order: 0)]
    [Export(typeof(PropertyViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class PropertyViewModel : ViewAware
    {
    }
}
