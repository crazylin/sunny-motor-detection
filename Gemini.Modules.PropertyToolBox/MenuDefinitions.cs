using System.ComponentModel.Composition;
using Gemini.Framework.Menus;
using Gemini.Modules.PropertyToolBox.Commands;

namespace Gemini.Modules.PropertyToolBox
{
    public static class MenuDefinitions
    {
        [Export] public static MenuItemDefinition ViewPropertyToolboxMenuItem =
            new CommandMenuItemDefinition<ViewPropertyToolboxCommandDefinition>(
                Gemini.Modules.MainMenu.MenuDefinitions.ViewToolsMenuGroup, 5);
    }
}