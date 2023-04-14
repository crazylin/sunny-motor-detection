using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Newtonsoft.Json;


namespace MotorDetection.SettingManager
{
    [Export(typeof(IAppSettingManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AppSettingManager : IAppSettingManager
    {
        private List<IAppSetting> _appSettings;

        [ImportingConstructor]
        public AppSettingManager()
        {
            Initialize();
        }

        public event EventHandler<AppSettingArgs> CurrentAppSettingChanged;

        public List<IAppSetting> AppSettings => _appSettings ?? (_appSettings = Initialize());

        public IAppSetting GetAppSettingByName(string extension)
        {
            return AppSettings.FirstOrDefault(l => l.Name.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        public List<IAppSetting> Initialize()
        {
            var appSettings = new List<IAppSetting>();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");


            //if (!Directory.Exists(path))
            //    return new List<IAppSetting>();

            // Add imported definitions
            foreach (var appSetting in IoC.GetAll<IAppSetting>())
            {
                var file = Path.Combine(path, appSetting.Name + ".json");
                if (File.Exists(file))
                {
                    var settings = new JsonSerializerSettings();
                    settings.TypeNameHandling = TypeNameHandling.All;
                    settings.Converters.Add(new IPAddressConverter());
                    settings.Converters.Add(new IPEndPointConverter());
                    var iAppSetting =
                        (IAppSetting)JsonConvert.DeserializeObject(File.ReadAllText(file), appSetting.GetType(),
                            settings);
                    iAppSetting?.Initialize();
                    appSettings.Add(iAppSetting);
                }
                else
                {
                    appSetting.Initialize();
                    appSettings.Add(appSetting);
                }
            }

            return appSettings;
        }

        public void SaveAppSetting(IAppSetting appSetting)
        {
            var settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.All;
            settings.Converters.Add(new IPAddressConverter());
            settings.Converters.Add(new IPEndPointConverter());
            settings.Formatting = Formatting.Indented;


            var json = JsonConvert.SerializeObject(appSetting, settings);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            var file = Path.Combine(path, appSetting.Name + ".json");
            File.WriteAllText(file, json);
            CurrentAppSettingChanged?.Invoke(this, new AppSettingArgs { AppSetting = appSetting });
        }

        public T GetAppSetting<T>() where T : IAppSetting, new()
        {
            var appSetting = new T();
            var tempAppSetting =
                AppSettings.FirstOrDefault(l =>
                    l != null && l.Name.Equals(appSetting.Name, StringComparison.OrdinalIgnoreCase));
            if (tempAppSetting == null)
            {
                AppSettings.Add(appSetting);
                return appSetting;
            }

            return (T)tempAppSetting;
        }
    }
}
