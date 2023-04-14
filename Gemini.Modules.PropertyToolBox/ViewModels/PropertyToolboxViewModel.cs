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
using Gemini.Modules.PropertyToolBox.Properties;
using Gemini.Modules.PropertyToolBox.Services;

namespace Gemini.Modules.PropertyToolBox.ViewModels
{
    [Export(typeof(IPropertyToolbox))]
    public class PropertyToolboxViewModel : Tool, IPropertyToolbox
    {
        private readonly BindableCollection<KeyValuePair<string, List<PropertyToolboxItemViewModel>>> _items;

        private readonly IPropertyToolboxService _toolboxService;


        [ImportingConstructor]
        public PropertyToolboxViewModel(IShell shell, IPropertyToolboxService toolboxService)
        {
            DisplayName = Resources.PropertyToolboxDisplayName;

            _items = new BindableCollection<KeyValuePair<string, List<PropertyToolboxItemViewModel>>>();

            _toolboxService = toolboxService;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            shell.ActiveDocumentChanged += (sender, e) => RefreshPropertyToolboxItems(shell);
            RefreshPropertyToolboxItems(shell);
        }

        public IObservableCollection<KeyValuePair<string, List<PropertyToolboxItemViewModel>>> Items => _items;

        public override PaneLocation PreferredLocation => PaneLocation.Right;

        public void RefreshPropertyToolboxItems(IShell shell)
        {
            _items.Clear();

            if (shell.ActiveItem == null)
                return;

            _items.AddRange(_toolboxService.GetPropertyToolboxItems(shell.ActiveItem.GetType())
                .OrderBy(x => x.Order)
                .Select(x => new PropertyToolboxItemViewModel(x))
                .Where(ovm => ovm.Visibility == Visibility.Visible)
                .GroupBy(ovm => ovm.Category)
                .Select(ovm => new KeyValuePair<string, List<PropertyToolboxItemViewModel>>(ovm.Key, ovm.ToList())));
        }

    }
}