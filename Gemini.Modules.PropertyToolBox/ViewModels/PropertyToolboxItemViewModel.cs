using System;
using System.Windows;
using Caliburn.Micro;
using Gemini.Modules.PropertyToolBox.Models;

namespace Gemini.Modules.PropertyToolBox.ViewModels
{
    public class PropertyToolboxItemViewModel
    {
        public PropertyToolboxItemViewModel(PropertyToolboxItem model)
        {
            Model = model;
            Model.Content = IoC.GetInstance(model.ItemType, null);
            
            //Visibility = (Visibility)Model.Content.GetType().GetProperty(nameof(Visibility))?.GetValue(Model.Content)!;
        }

        public PropertyToolboxItem Model { get; }

        public string Name => Model.Name;

        public virtual string Category => Model.Category;

        public virtual Uri? IconSource => Model.IconSource;

        public object Content => Model.Content;

        public Visibility Visibility { set; get; } = Visibility.Visible;
    }
}