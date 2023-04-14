using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gemini.Framework.Menus;
using MotorDetection.Welcome.Commands;

namespace MotorDetection.Welcome
{
    public class MenuDefinitions
    {
        [Export]
        public static MenuItemGroupDefinition ViewMenuGroup = new MenuItemGroupDefinition(
            Gemini.Modules.MainMenu.MenuDefinitions.ViewMenu, 10);

        [Export]
        public static MenuItemDefinition ViewMenuItem = new CommandMenuItemDefinition<ViewWelcomeCommandDefinition>(
            ViewMenuGroup, 0);

    }
}
