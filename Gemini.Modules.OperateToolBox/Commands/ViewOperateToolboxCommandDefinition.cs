using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Modules.OperateToolBox.Properties;

namespace Gemini.Modules.OperateToolBox.Commands
{
    [CommandDefinition]
    public class ViewOperateToolboxCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.OperateToolbox";

        public override string Name => CommandName;

        public override string Text => Resources.ViewOperateToolboxCommandText;

        public override string ToolTip => Resources.ViewOperateToolboxCommandToolTip;
    }
}
