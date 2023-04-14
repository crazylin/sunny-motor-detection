using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDetection.Daq.NI_DAQ.Models
{
    public class Device
    {
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


        public void Get()
        {
            int ret = 0;
            double sampleRate = 0;
            var range = new double[256];
            if (AIPhysicalChannels.Length > 0)
            {

                ret = NativeMethods.DAQmxGetDevAIMaxMultiChanRate(DeviceID, ref sampleRate);
                NativeMethods.ErrorCheck(ret);
                AIMaximumMultiChannelRate = sampleRate;

                ret = NativeMethods.DAQmxGetDevAIMaxSingleChanRate(DeviceID, ref sampleRate);
                NativeMethods.ErrorCheck(ret);
                AIMaximumSingleChannelRate = sampleRate;

                ret = NativeMethods.DAQmxGetDevAIMinRate(DeviceID, ref sampleRate);
                NativeMethods.ErrorCheck(ret);
                AIMinimumRate = sampleRate;



                ret = NativeMethods.DAQmxGetDevAIVoltageRngs(DeviceID, range, (uint)range.Length);
                NativeMethods.ErrorCheck(ret);
                AIVoltageRanges = range.Where(d => Math.Abs(d) > double.Epsilon).ToArray();

                int couplings = 0;
                ret = NativeMethods.DAQmxGetDevAICouplings(DeviceID, ref couplings);
                NativeMethods.ErrorCheck(ret);
                AICouplings = (NativeMethods.CouplingTypes)couplings;
            }

            if (AOPhysicalChannels.Length > 0)
            {
                ret = NativeMethods.DAQmxGetDevAOMaxRate(DeviceID, ref sampleRate);
                NativeMethods.ErrorCheck(ret);
                AOMaximumRate = sampleRate;

                ret = NativeMethods.DAQmxGetDevAOMinRate(DeviceID, ref sampleRate);
                NativeMethods.ErrorCheck(ret);
                AOMinimumRate = sampleRate;

                ret = NativeMethods.DAQmxGetDevAOVoltageRngs(DeviceID, range, (uint)range.Length);
                NativeMethods.ErrorCheck(ret);
                AOVoltageRanges = range.Where(d => Math.Abs(d) > double.Epsilon).ToArray();
            }

        }

        public int GetTimebaseDivisor()
        {
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateTask(Guid.NewGuid().ToString().Replace("-", ""), out var aiTaskHandle));
            // 添加一个模拟输入通道
            var chanName = Guid.NewGuid().ToString().Replace("-", "");
            var chan = AIPhysicalChannels[0];
            var minimumVoltage = AIVoltageRanges[0];
            var maximumVoltage = AIVoltageRanges[1];
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateAIVoltageChan(aiTaskHandle, chan, chanName,
                -1, minimumVoltage, maximumVoltage, (int)NativeMethods.AIVoltageUnits.Volts, IntPtr.Zero));
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCfgSampClkTiming(aiTaskHandle, "", GetMaxSampleRate(),
                (int)NativeMethods.Edge.Rising,
                (int)NativeMethods.SampleQuantityMode.FiniteSamples, (ulong)GetMaxSampleRate()));

            var ret = NativeMethods.DAQmxGetSampClkTimebaseDiv(aiTaskHandle, out var sampleClockTimebaseDivisor);
            NativeMethods.ErrorCheck(NativeMethods.DAQmxStopTask(aiTaskHandle));
            NativeMethods.ErrorCheck(NativeMethods.DAQmxClearTask(aiTaskHandle));
            return (int)sampleClockTimebaseDivisor;
            //daqSetting.AiSampleRate = sampleClockTimebaseRate / sampleClockTimebaseDivisor;
        }
        public double GetSampleClockTimebaseRate()
        {
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateTask(Guid.NewGuid().ToString().Replace("-", ""), out var aiTaskHandle));
            // 添加一个模拟输入通道
            var chanName = Guid.NewGuid().ToString().Replace("-", "");
            var chan = AIPhysicalChannels[0];
            var minimumVoltage = AIVoltageRanges[0];
            var maximumVoltage = AIVoltageRanges[1];
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCreateAIVoltageChan(aiTaskHandle, chan, chanName,
                -1, minimumVoltage, maximumVoltage, (int)NativeMethods.AIVoltageUnits.Volts, IntPtr.Zero));
            NativeMethods.ErrorCheck(NativeMethods.DAQmxCfgSampClkTiming(aiTaskHandle, "", GetMaxSampleRate(),
                (int)NativeMethods.Edge.Rising,
                (int)NativeMethods.SampleQuantityMode.FiniteSamples, (ulong)GetMaxSampleRate()));

            var ret = NativeMethods.DAQmxGetSampClkTimebaseRate(aiTaskHandle, out var sampleClockTimebaseRate);
            NativeMethods.ErrorCheck(NativeMethods.DAQmxStopTask(aiTaskHandle));
            NativeMethods.ErrorCheck(NativeMethods.DAQmxClearTask(aiTaskHandle));
            return sampleClockTimebaseRate;
        }
        public double GetMaxSampleRate()
        {
            return Math.Min(AIMaximumMultiChannelRate, AIMaximumSingleChannelRate);
        }


        public static string[] Devices
        {
            get
            {
                StringBuilder sb = new StringBuilder(1024);
                var ret = NativeMethods.DAQmxGetSysDevNames(sb, 1024);
                NativeMethods.ErrorCheck(ret);//NativeMethods
                if (string.IsNullOrWhiteSpace(sb.ToString()))
                    return new string[0];
                return sb.ToString().Split(',').Select(s => s.Trim()).ToArray();
            }
        }

        public static Device LoadDevice(string dev)
        {
            var device = new Device();
            device.DeviceID = dev;
            var sb = new StringBuilder(1024);
            var ret = NativeMethods.DAQmxGetDevProductType(dev, sb, 1024);
            NativeMethods.ErrorCheck(ret);
            device.ProductType = sb.ToString();

            uint serialNum = 0;
            ret = NativeMethods.DAQmxGetDevSerialNum(dev, ref serialNum);
            NativeMethods.ErrorCheck(ret);
            device.SerialNum = serialNum;

            ret = NativeMethods.DAQmxGetDevAIPhysicalChans(dev, sb, 1024);
            NativeMethods.ErrorCheck(ret);
            device.AIPhysicalChannels = sb.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).ToArray();

            ret = NativeMethods.DAQmxGetDevAOPhysicalChans(dev, sb, 1024);
            NativeMethods.ErrorCheck(ret);
            device.AOPhysicalChannels = sb.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim()).ToArray();

            
            return device;
        }
    }
}
