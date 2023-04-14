using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDetection.SettingManager
{
    public interface IAppSetting
    {
        string Name { get; }
        void Initialize();
    }
}
