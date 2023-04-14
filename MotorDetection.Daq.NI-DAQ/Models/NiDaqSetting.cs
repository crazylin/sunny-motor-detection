using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using MotorDetection.Daq.Models;
using MotorDetection.SettingManager;
using Newtonsoft.Json;

namespace MotorDetection.Daq.NI_DAQ.Models
{
    [Export(typeof(IAppSetting))]
    public class NiDaqSetting : BaseDaqSetting, IAppSetting
    {
        public Device Device { set; get; }
        public override double GetMaxAISampleRate()
        {
            if (AIChannelSettings == null)
                return Device.AIMaximumSingleChannelRate;
            var count = AIChannelSettings.Count(c => c.IsOn);
            var maxSampleRate =
                count > 1 ? Device.AIMaximumMultiChannelRate : Device.AIMaximumSingleChannelRate;
            return maxSampleRate;
        }

        public string[] AIPhysicalChannels { set; get; }
        public string[] AOPhysicalChannels { set; get; }

        public string Name => $"{DeviceID}-{ProductType}";
        public string DeviceID { set; get; }
        public string ProductType { set; get; }
        public uint SerialNum { set; get; }
        public double AIMaximumMultiChannelRate { set; get; }
        public double AIMaximumSingleChannelRate { set; get; }
        public double AIMinimumRate { set; get; }
        public double AOMaximumRate { set; get; }
        public double AOMinimumRate { set; get; }

        public double[] AIVoltageRanges { set; get; }
        public double[] AOVoltageRanges { set; get; }
        public NativeMethods.CouplingTypes AICouplings { set; get; }

    }
}
