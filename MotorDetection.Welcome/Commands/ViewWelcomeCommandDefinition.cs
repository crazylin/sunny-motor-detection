using Gemini.Framework.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MotorDetection.Welcome.Commands
{
    [CommandDefinition]
    public class ViewWelcomeCommandDefinition : CommandDefinition
    {
        public const string CommandName = "View.Home";

        public override string Name => CommandName;

        public override string Text => "欢迎页";

        public override string ToolTip => "欢迎页";

        public override Uri IconSource => new Uri("pack://application:,,,/QuickSA.Modules.Oscillography;component/Resources/Icons/RunUpdate_16x.png");

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<ViewWelcomeCommandDefinition>(new KeyGesture(Key.E, ModifierKeys.Control));
    }
}
