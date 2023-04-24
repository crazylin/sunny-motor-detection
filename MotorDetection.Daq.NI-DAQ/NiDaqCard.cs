using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using MotorDetection.Algorithm;
using MotorDetection.Daq.Models;
using MotorDetection.Daq.NI_DAQ.Models;

namespace MotorDetection.Daq.NI_DAQ
{
    [Export(typeof(IDaqCard))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NiDaqCard : IDaqCard
    {
        public event EventHandler<DaqDataArgs> DataReceived;

        private BaseAIChannelSetting[] _aiChannelSettings;
        private BaseAOChannelSetting[] _aoChannelSettings;
        private BaseDaqSetting _daqSetting;
        private CancellationTokenSource _cancellationTokenSource;

        private IntPtr _aiTaskHandle;
        private IntPtr _aoTaskHandle;
        private bool _thExist;
        private ConcurrentQueue<double[]> _cacheDataQueue;
        private double[] _readValue;
        private Task _readTask;
        public bool StartAiTask(BaseDaqSetting daqSetting, double sampleTime)
        {
            _daqSetting = daqSetting;

            var timeout = sampleTime * 2;
            var samplerNumber = (int)Math.Ceiling(daqSetting.AiSampleRate * sampleTime);

            // 创建一个新的DAQmx任务
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateTask(Guid.NewGuid().ToString().Replace("-", ""), out _aiTaskHandle));

            _aiChannelSettings = daqSetting.EffectiveAIChannelSettings.ToArray();
            for (int i = 0; i < _aiChannelSettings.Length; i++)
            {
                var channelSetting = (NiAIChannelSetting)_aiChannelSettings[i];
                var chan = channelSetting.Name;
                var chanName = Guid.NewGuid().ToString().Replace("-", "");


                var volRngs = channelSetting.VoltageRng.Split('~');
                var minimumVoltage = double.Parse(volRngs[0]);
                var maximumVoltage = double.Parse(volRngs[1]);

                if (channelSetting.AIMeasurementType == NativeMethods.AIMeasurementType.Voltage)
                {
                    // 添加一个模拟输入通道
                    NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateAIVoltageChan(_aiTaskHandle, chan, chanName,
                        -1, minimumVoltage, maximumVoltage, (int)NativeMethods.AIVoltageUnits.Volts, IntPtr.Zero));

                }
                else if (channelSetting.AIMeasurementType == NativeMethods.AIMeasurementType.VelocityIepeSensor)
                {
                    double sensitivity = 1.0;
                    NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateAIVelocityIEPEChan(_aiTaskHandle, chan, chanName,
                        (int)NativeMethods.AIterminalConfiguration.Differential,
                        minimumVoltage, maximumVoltage, (int)NativeMethods.AIVelocityUnits.MetersPerSecond, sensitivity,
                        (int)NativeMethods.AIVelocityIepeSensorSensitivityUnits.MillivoltsPerMillimeterPerSecond,
                        (int)NativeMethods.AIExcitationSource.Internal, 0.002, ""));
                }

                NativeMethods.ErrorCheck(NativeMethods.DAQmxSetAICoupling(_aiTaskHandle, chan, (int)channelSetting.AICoupling));
            }

            var maxBuffLen = Math.Max(samplerNumber, daqSetting.GetMaxAISampleRate());
            //设置缓冲区大小 设置与采样率相等  高采样率下内存可能会溢出
            //https://knowledge.ni.com/KnowledgeArticleDetails?id=kA00Z000000P9PkSAK&l=zh-CN
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCfgInputBuffer(_aiTaskHandle, (uint)(maxBuffLen * 2)));


            NativeMethods.ErrorCheck(NativeMethods.DAQmxCfgSampClkTiming(_aiTaskHandle, "", _daqSetting.AiSampleRate,
                (int)NativeMethods.Edge.Rising,
                (int)NativeMethods.SampleQuantityMode.FiniteSamples, (ulong)samplerNumber));


            //CheckError(NiNative.DAQmxGetSampClkTimebaseRate(_aiTaskHandle, out var sampleClockTimebaseRate));
            //CheckError(NiNative.DAQmxGetSampClkTimebaseDiv(_aiTaskHandle, out var sampleClockTimebaseDivisor));

            //if (Math.Abs(sampleClockTimebaseRate) < double.Epsilon || sampleClockTimebaseDivisor == 0)
            //{

            //}
            //else
            //{
            //    daqSetting.AiSampleRate = sampleClockTimebaseRate / sampleClockTimebaseDivisor;

            //}
            //CheckError(NiNative.DAQmxCfgSampClkTiming(_aiTaskHandle, "", _daqSetting.AiSampleRate,
            //    (int)NiNative.Edge.Rising,
            //    (int)NiNative.SampleQuantityMode.FiniteSamples, (ulong)samplerNumber));


            if (daqSetting.IsTriggerOn)
            {
                NativeMethods.ErrorCheck(NativeMethods.DAQmxCfgDigEdgeRefTrig(_aiTaskHandle, _daqSetting.TriggerChannel, (int)NativeMethods.Edge.Rising,
                    _daqSetting.TriggerLength));
            }

            //6366不支持重触发功能 只能循环开启任务来采集
            _thExist = true;
            _cacheDataQueue = new ConcurrentQueue<double[]>();
            _readValue = new double[_aiChannelSettings.Length * samplerNumber];
            uint numSampsPerChan = 0;

            _readTask = Task.Run(() =>
            {

                while (_thExist)
                {
                    NativeMethods.ErrorCheck(NativeMethods.DAQmxStartTask(_aiTaskHandle));

                    while (_thExist)
                    {
                        if (-200560 != NativeMethods.DAQmxWaitUntilTaskDone(_aiTaskHandle, 0.1))
                        {
                            NativeMethods.ErrorCheck(NativeMethods.DAQmxReadAnalogF64(_aiTaskHandle, samplerNumber, timeout,
                                (int)NativeMethods.FillMode.GroupByChannel, _readValue, _readValue.Length,
                                out int samplesPerChannelRead,
                                IntPtr.Zero));

                            if (_cacheDataQueue.Count > 2)
                                _cacheDataQueue.TryDequeue(out _);
                            _cacheDataQueue.Enqueue((double[])_readValue.Clone());
                            break;
                        }
                    }

                    // 停止任务
                    NativeMethods.ErrorCheck(NativeMethods.DAQmxStopTask(_aiTaskHandle));
                }
                NativeMethods.ErrorCheck(NativeMethods.DAQmxClearTask(_aiTaskHandle));
                _aiTaskHandle = IntPtr.Zero;
            });



            return true;
        }
        public bool StartAoTask(BaseDaqSetting daqSetting)
        {
            _daqSetting = daqSetting;
            _aoChannelSettings = daqSetting.EffectiveAOChannelSettings.ToArray();
            if (_aoChannelSettings.Length == 0)
                return false;

            // 创建一个新的DAQmx任务
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateTask(Guid.NewGuid().ToString().Replace("-", ""), out _aoTaskHandle));

            var sampleRate = daqSetting.AiSampleRate;//daqSetting.AoSampleRate;// 2000000d;// daqSetting.AoSampleRate;

            for (int i = 0; i < _aoChannelSettings.Length; i++)
            {
                var channelSetting = (NiAOChannelSetting)_aoChannelSettings[i];
                var chan = channelSetting.Name;
                var chanName = Guid.NewGuid().ToString().Replace("-", "");

                var volRngs = channelSetting.VoltageRng.Split('~');
                var minimumVoltage = double.Parse(volRngs[0]);
                var maximumVoltage = double.Parse(volRngs[1]);

                NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateAOVoltageChan(_aoTaskHandle, chan, chanName, minimumVoltage,
                    maximumVoltage, (int)NativeMethods.AOVoltageUnits.Volts, IntPtr.Zero));
            }


            int samplesPerChannelHasWrite = 0;
            var wData = new List<double>();
            foreach (var channel in _aoChannelSettings)
            {
                var sweepTime = channel.SweepTime;
                //if (channel.SweepMode == SweepMode.None)
                //    sweepTime = 1;

                var signalData = SignalGenerator.GenerateWaveform(sampleRate, channel.Frequency,
                    channel.FrequencyEnd,
                    channel.Amplitude, channel.Phase, channel.SignalGeneratorType,
                    channel.SweepMode, sweepTime);

                wData.AddRange(signalData);


            }

            NativeMethods.ErrorCheck(NativeMethods.DAQmxCfgSampClkTiming(_aoTaskHandle, "", sampleRate, (int)NativeMethods.Edge.Rising,
                (int)NativeMethods.SampleQuantityMode.ContinuousSamples, (ulong)(wData.Count / _aoChannelSettings.Length)));

            NativeMethods.ErrorCheck(NativeMethods.DAQmxWriteAnalogF64(_aoTaskHandle, wData.Count / _aoChannelSettings.Length, 1 /*true*/, 60,
                (int)NativeMethods.FillMode.GroupByChannel, wData.ToArray(), ref samplesPerChannelHasWrite, IntPtr.Zero));

            return true;
        }

        public void StopAiTask()
        {
            _thExist = false;
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            //CheckError(NiNative.DAQmxStopTask(_aiTaskHandle));


        }
        public void StopAoTask()
        {
            // 停止任务
            if (_aoTaskHandle != IntPtr.Zero)
            {
                NativeMethods.ErrorCheck(NativeMethods.DAQmxStopTask(_aoTaskHandle));
                NativeMethods.ErrorCheck(NativeMethods.DAQmxClearTask(_aoTaskHandle));
            }
            _aoTaskHandle = IntPtr.Zero;
        }
        public bool GetData(int sampleNumber, BaseAIChannelSetting[] channels, ref bool triggered, out int actSampleNumber, CancellationToken cancellationToken)
        {

            actSampleNumber = 0;
            triggered = false;
            if (_cacheDataQueue.TryDequeue(out var displayValue))
            {
                triggered = true;
                var samplesPerChannel = displayValue.Length / channels.Length;
                for (int i = 0; i < channels.Length; i++)
                {
                    int currentIndex = samplesPerChannel * i;

                    for (int j = currentIndex; j < currentIndex + samplesPerChannel; j++)
                    {
                        displayValue[j] *= channels[i].Calibration;
                    }

                    Array.Copy(displayValue, currentIndex, channels[i].TimeData, channels[i].TimeDataIndex,
                        samplesPerChannel);
                }
                actSampleNumber = samplesPerChannel;
                return true;
            }

            return false;
        }
    }
}
