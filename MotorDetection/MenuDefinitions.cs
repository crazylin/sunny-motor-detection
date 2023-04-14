using Gemini.Framework.Menus;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.ToolBars;

namespace MotorDetection
{
    public class MenuDefinitions
    {
        [Export]
        public static ExcludeMenuDefinition ExcludeFileMenu =
            new(Gemini.Modules.MainMenu.MenuDefinitions.FileMenu);
        [Export]
        public static ExcludeMenuDefinition ExcludeEditMenu =
            new(Gemini.Modules.MainMenu.MenuDefinitions.EditMenu);

        [Export]
        public static ExcludeMenuItemDefinition ExcludeViewHistoryMenuItem =
            new(Gemini.Modules.UndoRedo.MenuDefinitions.ViewHistoryMenuItem);
        [Export]
        public static ExcludeMenuItemDefinition ExcludeViewToolboxMenuItem =
            new(Gemini.Modules.Toolbox.MenuDefinitions.ViewToolboxMenuItem);


    }
}
