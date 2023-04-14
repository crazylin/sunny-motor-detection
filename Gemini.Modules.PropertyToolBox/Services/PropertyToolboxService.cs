using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Caliburn.Micro;
using Gemini.Modules.PropertyToolBox.Models;

namespace Gemini.Modules.PropertyToolBox.Services
{
    [Export(typeof(IPropertyToolboxService))]
    public class PropertyToolboxService : IPropertyToolboxService
    {
        private readonly Dictionary<Type, IEnumerable<PropertyToolboxItem>> _toolboxItems;

        public PropertyToolboxService()
        {
            _toolboxItems = AssemblySource.Instance
                .SelectMany(x =>
                    x.GetTypes().Where(y => y.GetCustomAttributes(typeof(PropertyToolboxItemAttribute), false).Any()))
                .Select(x =>
                {
                    var attribute =
                        (PropertyToolboxItemAttribute) x.GetCustomAttributes(typeof(PropertyToolboxItemAttribute), false)
                            .First();
                    return new PropertyToolboxItem
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

        public IEnumerable<PropertyToolboxItem> GetPropertyToolboxItems(Type documentType)
        {
            return _toolboxItems.TryGetValue(documentType, out var result) ? result : new List<PropertyToolboxItem>();
        }
    }
}