using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using Caliburn.Micro;
using Gemini.Framework.Threading;
using Gemini.Modules.Settings;
using Gemini.Modules.Settings.ViewModels;
using MotorDetection.Algorithm;
using MotorDetection.Daq.Models;
using MotorDetection.Daq.NI_DAQ.Models;
using MotorDetection.SettingManager;

namespace MotorDetection.Daq.NI_DAQ.Setting.ViewModels
{
    [Export(typeof(ISettingsEditor))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DaqSettingViewModel : Gemini.Modules.Utils.PropertyChangedBase, ISettingsEditor
    {
        public string SettingsPageName => "通道";
        public string SettingsPagePath => "采集";

        private readonly NiDaqSetting _niDaqSetting;
        private BindableCollection<Device> _devices;
        private BindableCollection<NativeMethods.AICoupling> _aiCouplings;
        private BindableCollection<string> _voltageRngs;
        private BindableCollection<string> _measurementUnits;
        private Device _selectedDevice;


        public BindableCollection<Device> Devices
        {
            set
            {
                _devices = value;
                NotifyOfPropertyChange(() => Devices);
            }
            get => _devices;
        }

        public BindableCollection<NativeMethods.AICoupling> AICouplings
        {
            set
            {
                _aiCouplings = value;
                NotifyOfPropertyChange(() => AICouplings);
            }
            get => _aiCouplings;
        }

        public BindableCollection<string> VoltageRngs
        {
            set
            {
                _voltageRngs = value;
                NotifyOfPropertyChange(() => VoltageRngs);
            }
            get => _voltageRngs;
        }

        public BindableCollection<string> AoVoltageRngs
        {
            set
            {
                _aoVoltageRngs = value;
                NotifyOfPropertyChange(() => AoVoltageRngs);
            }
            get => _aoVoltageRngs;
        }

        public BindableCollection<string> MeasurementUnits
        {
            set
            {
                _measurementUnits = value;
                NotifyOfPropertyChange(() => MeasurementUnits);
            }
            get => _measurementUnits;
        }

        public BindableCollection<NativeMethods.AIMeasurementType> AIMeasurementTypes
        {
            set
            {
                _aiMeasurementTypes = value;
                NotifyOfPropertyChange(() => AIMeasurementTypes);
            }
            get => _aiMeasurementTypes;
        }

 
        public BindableCollection<SignalGenerator.SignalGeneratorType> SignalGeneratorTypes
        {
            set
            {
                _signalGeneratorTypes = value;
                NotifyOfPropertyChange(() => SignalGeneratorTypes);
            }
            get => _signalGeneratorTypes;
        }

        public BindableCollection<SignalGenerator.SweepMode> SweepModes
        {
            set
            {
                _sweepModes = value;
                NotifyOfPropertyChange(() => SignalGeneratorTypes);
            }
            get => _sweepModes;
        }

        public Device SelectedDevice
        {
            set
            {
                _selectedDevice = value;
                NotifyOfPropertyChange(() => SelectedDevice);
            }
            get => _selectedDevice;
        }

        public BindableCollection<NiAIChannelSetting> AiChannelSettings
        {
            set
            {
                _aiChannelSettings = value;
                NotifyOfPropertyChange(() => AiChannelSettings);
            }
            get => _aiChannelSettings;
        }

        public BindableCollection<NiAOChannelSetting> AoChannelSettings
        {
            set
            {
                _aoChannelSettings = value;
                NotifyOfPropertyChange(() => AoChannelSettings);
            }
            get => _aoChannelSettings;
        }


        private List<double> _sampleRateList;
        private BindableCollection<NiAIChannelSetting> _aiChannelSettings;
        private BindableCollection<NativeMethods.AIMeasurementType> _aiMeasurementTypes;
        private BindableCollection<string> _aoVoltageRngs;
        private BindableCollection<NiAOChannelSetting> _aoChannelSettings;
        private BindableCollection<SignalGenerator.SignalGeneratorType> _signalGeneratorTypes;
        private BindableCollection<SignalGenerator.SweepMode> _sweepModes;

        public DaqSettingViewModel()
        {
            _niDaqSetting = IoC.Get<IAppSettingManager>().GetAppSetting<NiDaqSetting>();

            MeasurementUnits = new BindableCollection<string>(NiAIChannelSetting.GetMeasurementUnitName());
            AIMeasurementTypes = new BindableCollection<NativeMethods.AIMeasurementType>()
            {
                NativeMethods.AIMeasurementType.Voltage,
                NativeMethods.AIMeasurementType.VelocityIepeSensor
            };

            SignalGeneratorTypes =
                new BindableCollection<SignalGenerator.SignalGeneratorType>(Enum.GetValues(typeof(SignalGenerator.SignalGeneratorType)).Cast<SignalGenerator.SignalGeneratorType>());
            SweepModes =
                new BindableCollection<SignalGenerator.SweepMode>(Enum.GetValues(typeof(SignalGenerator.SweepMode)).Cast<SignalGenerator.SweepMode>());

            IoC.Get<SettingsViewModel>().ViewAttached += (s, e) => { Init(); };
            Init();

        }

        private void Init()
        {
            //强制拆装箱导致对象变了
            AiChannelSettings =
                new BindableCollection<NiAIChannelSetting>();
            foreach (var baseAiChannelSetting in _niDaqSetting.AIChannelSettings)
            {
                AiChannelSettings.Add((NiAIChannelSetting) baseAiChannelSetting);
            }
            AoChannelSettings =
                new BindableCollection<NiAOChannelSetting>();
            foreach (var baseAoChannelSetting in _niDaqSetting.AOChannelSettings)
            {
                AoChannelSettings.Add((NiAOChannelSetting)baseAoChannelSetting);
            }
            RefreshDevices();
        }


        public override void NotifyOfPropertyChange(string propertyName = null)
        {
            base.NotifyOfPropertyChange(propertyName);

            if (propertyName == nameof(SelectedDevice))
            {
                RefreshChannelSetting(SelectedDevice);
            }
        }

        public void RefreshDevices()
        {
            var deviceList = new List<Device>();
            foreach (var d in Device.Devices)
            {
                var device = Device.LoadDevice(d);
                if (device.AIPhysicalChannels.Length > 0)
                {
                    deviceList.Add(device);
                }
            }

            Devices = new BindableCollection<Device>(deviceList);

            if (!string.IsNullOrWhiteSpace(_niDaqSetting.Device?.DeviceID))
            {
                SelectedDevice = Devices.FirstOrDefault(d => d.DeviceID == _niDaqSetting.Device.DeviceID);
            }
            else
            {
                if (Devices.Count > 0)
                {
                    SelectedDevice = Devices[0];
                }
            }
        }

        private void RefreshChannelSetting(Device device)
        {
            if (device == null)
                return;
            device.Get();
            var aiCouplings = new List<NativeMethods.AICoupling>();
            if ((device.AICouplings & NativeMethods.CouplingTypes.DC) == NativeMethods.CouplingTypes.DC)
            {
                aiCouplings.Add(NativeMethods.AICoupling.DC);
            }

            if ((device.AICouplings & NativeMethods.CouplingTypes.AC) == NativeMethods.CouplingTypes.AC)
            {
                aiCouplings.Add(NativeMethods.AICoupling.AC);
            }

            if ((device.AICouplings & NativeMethods.CouplingTypes.Ground) == NativeMethods.CouplingTypes.Ground)
            {
                aiCouplings.Add(NativeMethods.AICoupling.Ground);
            }

            AICouplings = new BindableCollection<NativeMethods.AICoupling>(aiCouplings);

            var voltageRngs = new List<string>();
            for (int i = 0; i < device.AIVoltageRanges.Length; i += 2)
            {
                voltageRngs.Add($"{device.AIVoltageRanges[i]}~{device.AIVoltageRanges[i + 1]}");
            }

            VoltageRngs = new BindableCollection<string>(voltageRngs);


            var channelSettings = new List<NiAIChannelSetting>();
            var channels = _niDaqSetting.PhysicalAIChannelSettings;

            for (int i = 0; i < device.AIPhysicalChannels.Length; i++)
            {
                if (channels.Count > i)
                {
                    var oldChannel = (NiAIChannelSetting)channels[i];
                    oldChannel.Id = i + 1;
                    oldChannel.Name = device.AIPhysicalChannels[i];
                    oldChannel.AICoupling = AICouplings.Any(ac => ac == oldChannel.AICoupling)
                        ? AICouplings.FirstOrDefault(m => m == oldChannel.AICoupling)
                        : AICouplings[0];
                    oldChannel.VoltageRng = VoltageRngs.FirstOrDefault(vr => vr == oldChannel.VoltageRng) ??
                                            (VoltageRngs.Count > 0 ? VoltageRngs[VoltageRngs.Count - 1] : null);

                    oldChannel.MeasurementUnit = MeasurementUnits.FirstOrDefault(m => m == oldChannel.MeasurementUnit);

                    oldChannel.AICouplings = AICouplings;
                    oldChannel.VoltageRngs = VoltageRngs;
                    oldChannel.MeasurementUnits = MeasurementUnits;
                    oldChannel.AIMeasurementTypes = AIMeasurementTypes;
                    oldChannel.AIMeasurementType =
                        AIMeasurementTypes.FirstOrDefault(at => at == oldChannel.AIMeasurementType);
                    channelSettings.Add(oldChannel);
                }
                else
                {
                    var channel = new NiAIChannelSetting
                    {
                        Id = i + 1,
                        Name = device.AIPhysicalChannels[i],
                        IsOn = false,
                        TestPointName = (i + 1).ToString(),
                        Description = string.Empty,
                        AICoupling = AICouplings.Any(ai => ai == NativeMethods.AICoupling.DC)
                            ? AICouplings.FirstOrDefault(m => m == NativeMethods.AICoupling.DC)
                            : AICouplings[0],
                        VoltageRng = VoltageRngs.Count > 0 ? VoltageRngs[VoltageRngs.Count - 1] : null,
                        MeasurementUnit = MeasurementUnits.FirstOrDefault(mu => mu == "mm/s"),
                        Calibration = 1,
                        AICouplings = AICouplings,
                        VoltageRngs = VoltageRngs,
                        MeasurementUnits = MeasurementUnits,
                        AIMeasurementTypes = AIMeasurementTypes,
                        AIMeasurementType = AIMeasurementTypes[0]
                    };

                    channelSettings.Add(channel);
                }

            }

            AiChannelSettings = new BindableCollection<NiAIChannelSetting>(channelSettings);


            if (device.AOVoltageRanges != null)
            {
                var aoVoltageRngs = new List<string>();
                for (int i = 0; i < device.AOVoltageRanges.Length; i += 2)
                {
                    aoVoltageRngs.Add($"{device.AOVoltageRanges[i]}~{device.AOVoltageRanges[i + 1]}");
                }

                AoVoltageRngs = new BindableCollection<string>(aoVoltageRngs);


                var aoChannelSettings = new List<NiAOChannelSetting>();

                for (int i = 0; i < device.AOPhysicalChannels.Length; i++)
                {
                    if (_niDaqSetting.AOChannelSettings.Count > i)
                    {
                        var oldChannel = (NiAOChannelSetting)_niDaqSetting.AOChannelSettings[i];
                        oldChannel.Id = i + 1;
                        oldChannel.Name = device.AOPhysicalChannels[i];
                        oldChannel.VoltageRng = AoVoltageRngs.FirstOrDefault(vr => vr == oldChannel.VoltageRng) ??
                                                (AoVoltageRngs.Count > 0 ? AoVoltageRngs[AoVoltageRngs.Count - 1] : null);
                        oldChannel.VoltageRngs = AoVoltageRngs;
                        oldChannel.SignalGeneratorTypes = SignalGeneratorTypes;
                        oldChannel.SweepModes = SweepModes;
                        oldChannel.Amplitude = Math.Min(oldChannel.Amplitude, double.Parse(oldChannel.VoltageRng.Split('~')[1]));
                        oldChannel.SampleRate = device.AOMaximumRate;
                        aoChannelSettings.Add(oldChannel);
                    }
                    else
                    {
                        var channel = new NiAOChannelSetting
                        {
                            Id = i + 1,
                            Name = device.AOPhysicalChannels[i],
                            IsOn = false,
                            VoltageRng = AoVoltageRngs.Count > 0 ? AoVoltageRngs[AoVoltageRngs.Count - 1] : null,
                            SweepTime = 5,
                            Frequency = 10,
                            FrequencyEnd = 20000,
                            Phase = 0,
                            SampleRate = device.AOMaximumRate,
                            VoltageRngs = AoVoltageRngs,
                            SignalGeneratorTypes = SignalGeneratorTypes,
                            SweepModes = SweepModes
                        };
                        channel.Amplitude = double.Parse(channel.VoltageRng.Split('~')[1]) * 0.5;
                        aoChannelSettings.Add(channel);
                    }

                }
                AoChannelSettings = new BindableCollection<NiAOChannelSetting>(aoChannelSettings);
            }
            else
            {
                AoChannelSettings = new BindableCollection<NiAOChannelSetting>();
            }

            var tempSampleRateList = new List<double>();
            var divisor = device.GetTimebaseDivisor();
            var timeBase = device.GetSampleClockTimebaseRate();
            if (divisor == 0)
            {
                for (int i = -2; i < 12; i++)
                {
                    var num = 256 * Math.Pow(10, i);
                    tempSampleRateList.Add(num);
                    num = 512 * Math.Pow(10, i);
                    tempSampleRateList.Add(num);
                    num = 1024 * Math.Pow(10, i);
                    tempSampleRateList.Add(num);
                    num = 1280 * Math.Pow(10, i);
                    tempSampleRateList.Add(num);
                    num = 2048 * Math.Pow(10, i);
                    tempSampleRateList.Add(num);
                }
            }
            else
            {
                var divisors = new List<int>();
                divisors.Add(2);
                divisors.Add(5);
                tempSampleRateList.AddRange(GetSampleRateListEx(new List<double>() { timeBase }, divisor, divisors));
            }

            tempSampleRateList = tempSampleRateList.Distinct().OrderBy(t => t).ToList();
            var maxSampleRate = _selectedDevice.GetMaxSampleRate();
            if (device.Name.Contains("6363"))
            {
                maxSampleRate = 500000;
            }
            tempSampleRateList = tempSampleRateList
                .Where(s => s < maxSampleRate && s >= _selectedDevice.AIMinimumRate).ToList();
            tempSampleRateList.Add(maxSampleRate);

            _sampleRateList = new List<double>(tempSampleRateList);
        }

        public void ApplyChanges()
        {
            var removeChannels = _aiChannelSettings.Where(ais => ais.IsVirtual).ToArray();
            _aiChannelSettings.RemoveRange(removeChannels);

            //foreach (var niAiChannelSetting in _aiChannelSettings)
            //{
            //    niAiChannelSetting.ShowName = niAiChannelSetting.TimeDataType.GetDisplay();
            //}

            _niDaqSetting.Device = SelectedDevice;
            _niDaqSetting.AIChannelSettings = new BindableCollection<BaseAIChannelSetting>(_aiChannelSettings);
            _niDaqSetting.AOChannelSettings = new BindableCollection<BaseAOChannelSetting>(_aoChannelSettings);
            _niDaqSetting.SampleRates = new BindableCollection<double>(_sampleRateList);

            _niDaqSetting.AoSampleRate = _selectedDevice.AOMaximumRate;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (_niDaqSetting.SampleRates.All(sr => sr != _niDaqSetting.AiSampleRate))
            {
                _niDaqSetting.AiSampleRate = _niDaqSetting.SampleRates.ElementAt(_niDaqSetting.SampleRates.Count - 1);
            }
            IoC.Get<IAppSettingManager>().SaveAppSetting(_niDaqSetting);
        }

        private List<double> GetSampleRateListEx(List<double> sampleRateList, int baseValue, List<int> divisors)
        {
            var resultList = new List<double>();
            foreach (var s in sampleRateList)
            {
                foreach (var d in divisors)
                {
                    var subSampleRateList = GetSampleRateList(s, baseValue, d);
                    resultList.AddRange(subSampleRateList);
                    if (subSampleRateList.Count > 0)
                        resultList.AddRange(GetSampleRateListEx(subSampleRateList, baseValue, divisors));
                }
            }
            return resultList;
        }
        private List<double> GetSampleRateList(double timeBase, int baseValue, int divisor)
        {
            var list = new List<double>();
            while (true)
            {
                var num = timeBase / baseValue;
                if (num < 1)
                    break;
                if (int.TryParse(num.ToString(), out _))
                    list.Add(num);
                baseValue *= divisor;
            }

            return list;
        }
    }
}
