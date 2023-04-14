using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MotorDetection.Daq.Models;
using Newtonsoft.Json;

namespace MotorDetection.Daq.NI_DAQ.Models
{
    public class NiAOChannelSetting : BaseAOChannelSetting
    {
        public string VoltageRng { set; get; }

        [JsonIgnore] public BindableCollection<string> VoltageRngs { set; get; } = new BindableCollection<string>();
    }
}
