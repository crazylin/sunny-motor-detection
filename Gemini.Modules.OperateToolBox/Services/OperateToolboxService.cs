using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Modules.OperateToolBox.Models;

namespace Gemini.Modules.OperateToolBox.Services
{
    [Export(typeof(IOperateToolboxService))]
    public class OperateToolboxService : IOperateToolboxService
    {
        private readonly Dictionary<Type, IEnumerable<OperateToolboxItem>> _toolboxItems;

        public OperateToolboxService()
        {
            _toolboxItems = AssemblySource.Instance
                .SelectMany(x =>
                    x.GetTypes().Where(y => y.GetCustomAttributes(typeof(OperateToolboxItemAttribute), false).Any()))
                .Select(x =>
                {
                    var attribute =
                        (OperateToolboxItemAttribute) x.GetCustomAttributes(typeof(OperateToolboxItemAttribute), false)
                            .First();
                    return new OperateToolboxItem
                    {
                        DocumentType = attribute.DocumentType,
                        Name = attribute.Name,
                        Category = attribute.Category,
                        IconSource = string.IsNullOrWhiteSpace(attribute.IconSource)
                            ? null
                            : new Uri(attribute.IconSource),
                        ItemType = x,
                       // Content = attribute.Content,
                        Order = attribute.Order
                    };
                })
                .GroupBy(x => x.DocumentType)
                .ToDictionary(x => x.Key, x => x.AsEnumerable());
        }

        public IEnumerable<OperateToolboxItem> GetOperateToolboxItems(Type documentType)
        {
            return _toolboxItems.TryGetValue(documentType, out var result) ? result : new List<OperateToolboxItem>();
        }
    }
}