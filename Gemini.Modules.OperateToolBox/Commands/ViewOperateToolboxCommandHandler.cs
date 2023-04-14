using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Framework.Services;
using Gemini.Framework.Threading;

namespace Gemini.Modules.OperateToolBox.Commands
{
    [CommandHandler]
    public class ViewOperateToolboxCommandHandler : CommandHandlerBase<ViewOperateToolboxCommandDefinition>
    {
        private readonly IShell _shell;

        [ImportingConstructor]
        public ViewOperateToolboxCommandHandler(IShell shell)
        {
            _shell = shell;
        }

        public override Task Run(Command command)
        {
            _shell.ShowTool<IOperateToolbox>();
            return TaskUtility.Completed;
        }
    }
}
