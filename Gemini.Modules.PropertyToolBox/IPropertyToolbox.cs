using Gemini.Framework;
using Gemini.Framework.Services;

namespace Gemini.Modules.PropertyToolBox
{
    public interface IPropertyToolbox : ITool
    {
        void RefreshPropertyToolboxItems(IShell shell);
    }
}