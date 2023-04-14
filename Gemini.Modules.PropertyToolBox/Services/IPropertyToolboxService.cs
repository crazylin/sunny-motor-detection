using System;
using System.Collections.Generic;
using Gemini.Modules.PropertyToolBox.Models;

namespace Gemini.Modules.PropertyToolBox.Services
{
    public interface IPropertyToolboxService
    {
        IEnumerable<PropertyToolboxItem> GetPropertyToolboxItems(Type documentType);
    }
}