using System;

namespace Gemini.Modules.PropertyToolBox.Models
{
    public class PropertyToolboxItem
    {
        public Type DocumentType { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public Uri? IconSource { get; set; }
        public Type ItemType { get; set; }
        public object Content { set; get; }
        public int Order { set; get; }
    }
}