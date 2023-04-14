using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MotorDetection.Algorithm;
using Newtonsoft.Json;


namespace MotorDetection.Daq.Models
{
    public class BaseAOChannelSetting : Gemini.Modules.Utils.PropertyChangedBase
    {
        public int Id { set; get; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { set; get; }

        public string ShowName { set; get; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsOn { set; get; }

        public bool IsVirtual { set; get; } = false;

        /// <summary>
        /// 信号类型
        /// </summary>
        public SignalGenerator.SignalGeneratorType SignalGeneratorType { set; get; } = SignalGenerator.SignalGeneratorType.Sine;

        /// <summary>
        /// 扫频类型
        /// </summary>
        public SignalGenerator.SweepMode SweepMode { set; get; } = SignalGenerator.SweepMode.None;
        /// <summary>
        /// 频率或起始频率
        /// </summary>
        public double Frequency { set; get; }
        /// <summary>
        /// 结束频率
        /// </summary>
        public double FrequencyEnd { set; get; }

        /// <summary>
        /// 幅值
        /// </summary>
        public double Amplitude { set; get; }
        /// <summary>
        /// 相位
        /// </summary>
        public double Phase { set; get; }

        /// <summary>
        /// 扫速
        /// </summary>
        public double SweepTime { set; get; } = 1;

        /// <summary>
        /// 硬件支持的最大采样率
        /// </summary>
        public double SampleRate { set; get; }
        ///// <summary>
        ///// 信号衰减率
        ///// </summary>
        //public double Attenuation { set; get; } = 0.1;

        [JsonIgnore]
        public BindableCollection<SignalGenerator.SignalGeneratorType> SignalGeneratorTypes { set; get; } = new BindableCollection<SignalGenerator.SignalGeneratorType>();

        [JsonIgnore]
        public BindableCollection<SignalGenerator.SweepMode> SweepModes { set; get; } = new BindableCollection<SignalGenerator.SweepMode>();
    }
}
