using System;
using Gemini.Modules.Utils;

namespace Gemini.Modules.OperateToolBox
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OperateToolboxItemAttribute : Attribute
    {
        public OperateToolboxItemAttribute(Type documentType, string name, string category, int order, string iconSource = null, object content = null, Type resourceType = null)
        {
            DocumentType = documentType;
            Name = resourceType == null ? name : TranslationUtil.GetTranslate(name, resourceType);
            Category = resourceType == null ? category : TranslationUtil.GetTranslate(category, resourceType);
            Order = order;
            IconSource = iconSource;
            Content = content;
        }

        public Type DocumentType { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string IconSource { get; set; }
        public object Content { set; get; }
        public int Order { set; get; }
    }
}