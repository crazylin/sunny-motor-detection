using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Caliburn.Micro;
using MotorDetection.Algorithm;
using MotorDetection.SettingManager;
using Newtonsoft.Json;



namespace MotorDetection.Daq.Models
{

    public class BaseDaqSetting : Gemini.Modules.Utils.PropertyChangedBase,IAppSetting
    {
        public virtual string Name { get; } = "DaqSetting";
        public void Initialize()
        {

        }

        private BindableCollection<double> _sampleRates = new BindableCollection<double>();
        private double _bandwidth;
        private double _maxBandwidth;
        private double _resolution;
        private double _sampleRate;
        private int _sampleNumber = 1024;
        private BindableCollection<BaseAIChannelSetting> _aiChannelSettings = new BindableCollection<BaseAIChannelSetting>();
        private BindableCollection<BaseAOChannelSetting> _aoChannelSettings = new BindableCollection<BaseAOChannelSetting>();
        private bool _ignoreDc;
        private double _aoSampleRate;

        public double AiSampleRate
        {
            set
            {
                _sampleRate = value;
                NotifyOfPropertyChange(() => AiSampleRate);
            }
            get => _sampleRate;
        }

        public double AoSampleRate
        {
            set
            {
                _aoSampleRate = value;
                NotifyOfPropertyChange(() => AoSampleRate);
            }
            get => _aoSampleRate;
        }

        public int SampleNumber
        {
            set
            {
                _sampleNumber = value;
                NotifyOfPropertyChange(() => SampleNumber);
            }
            get => _sampleNumber;
        }

        public int RefreshSampleNumber { set; get; }

        public BindableCollection<double> SampleRates
        {
            set
            {
                _sampleRates = value;
                NotifyOfPropertyChange(() => SampleRates);
            }
            get => _sampleRates;
        }

        public BindableCollection<BaseAIChannelSetting> AIChannelSettings
        {
            set
            {
                _aiChannelSettings = value;
                NotifyOfPropertyChange(() => AIChannelSettings);
                NotifyOfPropertyChange(() => EffectiveAIChannelSettings);
                NotifyOfPropertyChange(() => ActivatedAIChannelSettings);
                
            }
            get => _aiChannelSettings;
        }

        public BindableCollection<BaseAOChannelSetting> AOChannelSettings
        {
            set
            {
                _aoChannelSettings = value;
                NotifyOfPropertyChange(() => AOChannelSettings);
                NotifyOfPropertyChange(() => EffectiveAOChannelSettings);
                NotifyOfPropertyChange(() => ActivatedAOChannelSettings);
            }
            get => _aoChannelSettings;
        }

        public double Bandwidth
        {
            set
            {
                _bandwidth = value;
                NotifyOfPropertyChange(() => Bandwidth);
            }
            get => _bandwidth;
        }

        public double MaxBandwidth
        {
            set
            {
                _maxBandwidth = value;
                NotifyOfPropertyChange(() => MaxBandwidth);
            }
            get => _maxBandwidth;
        }

        public double Resolution
        {
            set
            {
                _resolution = value;
                NotifyOfPropertyChange(() => Resolution);
            }
            get => _resolution;
        }

        public bool IgnoreDC
        {
            set
            {
                _ignoreDc = value;
                NotifyOfPropertyChange(() => IgnoreDC);
            }
            get => _ignoreDc;
        }

        public WindowFunction WindowFunction { set; get; } = WindowFunction.Rectangle;
        public AverageMode AverageMode { set; get; } = AverageMode.PeakHolding;


        public BaseAIChannelSetting TriggerAiChannelSetting { set; get; }
        public bool IsTriggerOn { set; get; } = false;
        public TriggerMode TriggerMode { set; get; } = TriggerMode.RisingEdge;
        public double TriggerValue { set; get; } = 10;

        public int TriggerLength { set; get; } = 10;

        public int DelayTime { set; get; }

        public int TriggerAverageTimes { set; get; } = 1;

        public TriggerType TriggerType { set; get; } = TriggerType.Hardware;

        public string TriggerChannel { set; get; }

        public virtual double GetMaxAISampleRate()
        {
            return double.MaxValue;
        }

        [JsonIgnore]
        public List<BaseAIChannelSetting> EffectiveAIChannelSettings => AIChannelSettings.Where(aic => aic.IsOn && !aic.IsVirtual).ToList();
        [JsonIgnore]
        public List<BaseAIChannelSetting> ActivatedAIChannelSettings => AIChannelSettings.Where(aic => aic.IsOn).ToList();
        [JsonIgnore]
        public List<BaseAIChannelSetting> PhysicalAIChannelSettings => AIChannelSettings.Where(aic => !aic.IsVirtual).ToList();
        [JsonIgnore]
        public List<BaseAIChannelSetting> VirtualAIChannelSettings => AIChannelSettings.Where(aic => aic.IsOn && aic.IsVirtual).ToList();
        [JsonIgnore]
        public List<BaseAOChannelSetting> EffectiveAOChannelSettings => AOChannelSettings.Where(aic => aic.IsOn && !aic.IsVirtual).ToList();
        [JsonIgnore]
        public List<BaseAOChannelSetting> ActivatedAOChannelSettings => AOChannelSettings.Where(aic => aic.IsOn).ToList();

        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            base.NotifyOfPropertyChange(propertyName);

            if (propertyName == nameof(AiSampleRate) || propertyName == nameof(SampleNumber))
            {
                MaxBandwidth = AiSampleRate / 2;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Bandwidth >= MaxBandwidth || Bandwidth == 0)
                {
                    Bandwidth = MaxBandwidth;
                }

                Resolution = AiSampleRate / SampleNumber;

            }
        }

    }
}
