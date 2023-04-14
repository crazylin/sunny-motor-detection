using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;

namespace Gemini.Modules.PropertyToolBox.Commands
{
    [CommandHandler]
    public class ViewPropertyToolboxCommandHandler : CommandHandlerBase<ViewPropertyToolboxCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public ViewPropertyToolboxCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool<IPropertyToolbox>();
            return TaskUtility.Completed;
        }
    }
}
