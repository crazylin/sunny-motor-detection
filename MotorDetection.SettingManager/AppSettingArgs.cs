using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDetection.SettingManager
{
    public class AppSettingArgs : EventArgs
    {
        public IAppSetting AppSetting { set; get; }
    }
}
