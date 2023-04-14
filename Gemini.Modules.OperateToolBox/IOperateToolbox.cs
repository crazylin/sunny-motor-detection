using Gemini.Framework;
using Gemini.Framework.Services;

namespace Gemini.Modules.OperateToolBox
{
    public interface IOperateToolbox : ITool
    {
        void RefreshOperateToolboxItems(IShell shell);
    }
}