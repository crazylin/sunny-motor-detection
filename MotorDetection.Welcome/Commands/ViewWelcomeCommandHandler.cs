using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;
using Gemini.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDetection.Welcome.ViewModels;

namespace MotorDetection.Welcome.Commands
{
    [CommandHandler]
    public class ViewWelcomeCommandHandler : CommandHandlerBase<ViewWelcomeCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public ViewWelcomeCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override async Task Run(Command command)
        {
            await _shell.OpenDocumentAsync((IDocument)IoC.GetInstance(typeof(WelcomeViewModel), null));
        }
    }
}
