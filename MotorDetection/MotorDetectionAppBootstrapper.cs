using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MotorDetection
{
    public class MotorDetectionAppBootstrapper : Gemini.AppBootstrapper
    {
        public override bool IsPublishSingleFileHandled => false;

        //protected override IEnumerable<Assembly?> PublishSingleFileBypassAssemblies
        //{
        //    get
        //    {
        //        yield return Assembly.GetAssembly(typeof(Gemini.AppBootstrapper)); // GeminiWpf
        //        yield return Assembly.GetAssembly(typeof(Gemini.Modules.Output.IOutput));
        //        yield return Assembly.GetAssembly(typeof(Gemini.Modules.OperateToolBox.IOperateToolbox));
        //        yield return Assembly.GetAssembly(typeof(Gemini.Modules.PropertyToolBox.IPropertyToolbox));
        //    }
        //}

        protected override void BindServices(CompositionBatch batch)
        {
            base.BindServices(batch);
        }
    }
}
