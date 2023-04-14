using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Commands;
using Gemini.Modules.PropertyToolBox.Properties;

namespace Gemini.Modules.PropertyToolBox.Commands
{
    [CommandDefinition]
    public class ViewPropertyToolboxCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.PropertyToolbox";

        public override string Name => CommandName;

        public override string Text => Resources.ViewPropertyToolboxCommandText;

        public override string ToolTip => Resources.ViewPropertyToolboxCommandToolTip;
    }
}
