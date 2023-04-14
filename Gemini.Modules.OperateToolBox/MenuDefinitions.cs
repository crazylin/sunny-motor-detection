using System.ComponentModel.Composition;
using Gemini.Framework.Menus;
using Gemini.Modules.OperateToolBox.Commands;

namespace Gemini.Modules.OperateToolBox
{
    public static class MenuDefinitions
    {
        [Export] public static MenuItemDefinition ViewOperateToolboxMenuItem =
            new CommandMenuItemDefinition<ViewOperateToolboxCommandDefinition>(
                Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 4);
    }
}