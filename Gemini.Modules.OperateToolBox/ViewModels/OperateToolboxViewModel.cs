using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Modules.OperateToolBox.Properties;
using Gemini.Modules.OperateToolBox.Services;

namespace Gemini.Modules.OperateToolBox.ViewModels
{
    [Export(typeof(IOperateToolbox))]
    public sealed class OperateToolboxViewModel : Tool, IOperateToolbox
    {
        private readonly BindableCollection<KeyValuePair<string, List<OperateToolboxItemViewModel>>> _items;

        private readonly IOperateToolboxService _toolboxService;

        [ImportingConstructor]
        public OperateToolboxViewModel(IShell shell, IOperateToolboxService toolboxService)
        {
            DisplayName = Resources.OperateToolboxDisplayName;

            _items = new BindableCollection<KeyValuePair<string, List<OperateToolboxItemViewModel>>>();

            _toolboxService = toolboxService;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            shell.ActiveDocumentChanged += (sender, e) => RefreshOperateToolboxItems(shell);

            RefreshOperateToolboxItems(shell);
        }

        public IObservableCollection<KeyValuePair<string, List<OperateToolboxItemViewModel>>> Items => _items;

        public override PaneLocation PreferredLocation => PaneLocation.Left;

        public void RefreshOperateToolboxItems(IShell shell)
        {
            _items.Clear();

            if (shell.ActiveItem == null)
                return;

            _items.AddRange(_toolboxService.GetOperateToolboxItems(shell.ActiveItem.GetType())
                .OrderBy(x => x.Order)
                .Select(x => new OperateToolboxItemViewModel(x))
                .Where(ovm => ovm.Visibility == Visibility.Visible)
                .GroupBy(ovm => ovm.Category)
                .Select(ovm => new KeyValuePair<string, List<OperateToolboxItemViewModel>>(ovm.Key, ovm.ToList())));
        }
    }
}