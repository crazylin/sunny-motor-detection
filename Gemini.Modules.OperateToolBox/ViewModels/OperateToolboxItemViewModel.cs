using System;
using System.Windows;
using Caliburn.Micro;
using Gemini.Modules.OperateToolBox.Models;

namespace Gemini.Modules.OperateToolBox.ViewModels
{
    public class OperateToolboxItemViewModel
    {
        public OperateToolboxItemViewModel(OperateToolboxItem model)
        {
            Model = model;
            Model.Content = IoC.GetInstance(model.ItemType, null);
            //Visibility = (Visibility)Model.Content.GetType().GetProperty(nameof(Visibility))?.GetValue(Model.Content)!;
        }

        public OperateToolboxItem Model { get; }

        public string Name => Model.Name;

        public virtual string Category => Model.Category;

        public virtual Uri? IconSource => Model.IconSource;

        public object Content => Model.Content;

        public Visibility Visibility { set; get; } = Visibility.Visible; //反射获取 UserControl 的 Visibility 属性
    }
}