using System;
using System.Collections.Generic;
using Gemini.Modules.OperateToolBox.Models;

namespace Gemini.Modules.OperateToolBox.Services
{
    public interface IOperateToolboxService
    {
        IEnumerable<OperateToolboxItem> GetOperateToolboxItems(Type documentType);
    }
}