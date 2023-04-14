using Caliburn.Micro;
using Gemini.Framework.Services;
using Gemini.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDetection.Welcome.ViewModels;

namespace MotorDetection.Welcome
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        public override IEnumerable<IDocument> DefaultDocuments
        {
            get { yield return IoC.Get<WelcomeViewModel>(); }
        }

        public override async Task PostInitializeAsync()
        {
            await Shell.OpenDocumentAsync(IoC.Get<WelcomeViewModel>());
        }
    }
}
