using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDetection.SettingManager
{
    public interface IAppSettingManager
    {
        event EventHandler<AppSettingArgs> CurrentAppSettingChanged;
        List<IAppSetting> AppSettings { get; }

        IAppSetting GetAppSettingByName(string extension);

        T GetAppSetting<T>() where T : IAppSetting, new();

        List<IAppSetting> Initialize();

        void SaveAppSetting(IAppSetting appSetting);
    }
}
