using Gemini.Modules.OperateToolBox;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MotorDetection.Welcome.ViewModels
{
    [OperateToolboxItem(
        documentType: typeof(WelcomeViewModel),
        name: "欢迎界面工具",
        category: "工具1",
        order: 0)]
    [Export(typeof(ToolBox1ViewModel))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ToolBox1ViewModel : ViewAware
    {
    }
}
