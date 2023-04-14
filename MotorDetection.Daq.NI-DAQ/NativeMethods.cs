using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MotorDetection.Daq.NI_DAQ
{
    public class NativeMethods
    {

        // 声明DAQmx ANSI C库中的函数
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCreateTask(string taskName, out IntPtr taskHandle);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCreateAIVoltageChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, int terminalConfig, double minVal, double maxVal, int units, IntPtr reserved);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCfgSampClkTiming(IntPtr taskHandle, string source, double rate, int activeEdge, int sampleMode, ulong sampsPerChan);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxStartTask(IntPtr taskHandle);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxStopTask(IntPtr taskHandle);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxClearTask(IntPtr taskHandle);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCreateDOChan(IntPtr taskHandle, string lines, string nameToAssignToLines, int lineGrouping);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxStartTriggerCfgDigEdge(IntPtr taskHandle, string triggerSource, int triggerEdge);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxReferenceTriggerCfgDigEdge(IntPtr taskHandle, string triggerSource, int triggerEdge);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxIsTaskDone(IntPtr taskHandle, out int isTaskDone);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCfgDigEdgeRefTrig(IntPtr taskHandle, string triggerSource, int triggerEdge, int pretriggerSamples);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxWriteDigitalLines(IntPtr taskHandle, int numSampsPerChan, int autoStart, double timeout, int dataLayout, byte[] writeArray, out int sampsPerChanWritten, IntPtr reserved);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxReadAnalogF64(IntPtr taskHandle, int numSampsPerChan, double timeout, int fillMode, double[] readArray, int arraySizeInSamps, out int sampsPerChanRead, IntPtr reserved);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetReadAvailSampPerChan(IntPtr taskHandle, ref uint numSampsPerChan);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCfgDigEdgeStartTrig(IntPtr taskHandle, string source, int edge);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxSetRefTrigRetriggerable(IntPtr taskHandle, bool retriggerable);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxSetStartTrigRetriggerable(IntPtr taskHandle, bool retriggerable);


        [DllImport("nicaiu.dll")]
        public static extern int DAQmxWaitUntilTaskDone(IntPtr taskHandle, double timeout);


        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCreateVelocityIepeChannel(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel, int terminalConfig, double minVal, double maxVal, int units, double sensitivity, double currentExcitSource, int voltageExcitSource, int voltageExcitValue, int filterMode, double cutoffFreq, string customScaleName);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCreateAIVelocityIEPEChan(
            IntPtr taskHandle,
            string physicalChannel,
            string nameToAssignToChannel, int terminalConfig, double minVal, double maxVal, int units,
            double sensitivity, int sensitivityUnits, int currentExcitSource, double currentExcitVal,
            string customScaleName);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxSetAICoupling(IntPtr taskHandle, string channel, int coupling);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCfgInputBuffer(IntPtr taskHandle, uint numSampsPerChan);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetSampClkTimebaseRate(IntPtr taskHandle, out double data);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetSampClkTimebaseDiv(IntPtr taskHandle, out uint data);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetPhysicalChans(string deviceName, StringBuilder chanList, int bufSize);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetExtendedErrorInfo(StringBuilder errorString, int bufferSize);


        [DllImport("nicaiu.dll")]
        public static extern int DAQmxCreateAOVoltageChan(IntPtr taskHandle, string physicalChannel, string nameToAssignToChannel,
            double minVal, double maxVal, int units, IntPtr customScaleName);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxWriteAnalogScalarF64(IntPtr taskHandle, bool autoStart, double timeout, double value0, double value1, IntPtr reserved);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxWriteAnalogF64(System.IntPtr taskHandle, int numSampsPerChan, uint autoStart,
            double timeout, uint dataLayout, double[] writeArray, ref int sampsPerChanWritten, System.IntPtr reserved/* ref uint reserved*/);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAIMaxMultiChanRate(string device, ref double data);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAIMaxSingleChanRate(string device, ref double data);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAIMinRate(string device, ref double data);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAIVoltageRngs(string device, double[] data, uint arraySizeInElements);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAICouplings(string device, ref int data);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAOMaxRate(string device, ref double data);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAOMinRate(string device, ref double data);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAOVoltageRngs(string device, double[] data, uint arraySizeInElements);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetSysDevNames(StringBuilder data, uint bufferSize);

        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevProductType(string device, StringBuilder data, uint bufferSize);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevSerialNum( string device, ref uint data);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAIPhysicalChans(string device, StringBuilder data, uint bufferSize);
        [DllImport("nicaiu.dll")]
        public static extern int DAQmxGetDevAOPhysicalChans(string device, StringBuilder data, uint bufferSize);
        public static void ErrorCheck(int code)
        {
            if (code < 0)
            {
                StringBuilder sb = new StringBuilder(1024);
                DAQmxGetExtendedErrorInfo(sb, 1024);
                Debug.WriteLine(sb.ToString());
                if (code == -200088 || code == -200983 || code == -200197)
                {
                    return;
                }

                //MessageBox.Show(Application.Current.MainWindow, sb.ToString(), "采集卡：警告：", MessageBoxButton.OK,
                //    MessageBoxImage.Warning);
                throw new Exception($"NI-DAQmx Error:\nCode:{code}\n{sb.ToString()}");
            }
        }
        public enum AIVoltageUnits
        {
            Volts = 10348,
            Custom = 10065,
            FromTEDS = 12519,
            FromCustomScale = 10065, // DAQmx_Val_FromCustomScale
            FromEngUnits = 10143, // DAQmx_Val_FromEngUnits
            FromTEDSUnits = 12526 // DAQmx_Val_FromTEDSUnits
        }
        public enum AIterminalConfiguration
        {
            Default = -1,
            Rse = 10083,
            Nrse = 10078,
            Differential = 10106,
            Pseudodifferential = 12529


        }
        public enum AIVelocityUnits
        {
            MetersPerSecond = 10301,
            FeetPerSecond = 10302,
            InchesPerSecond = 10303,
            MillimetersPerSecond = 10304,
            MicroinchesPerSecond = 10305
        }
        public enum AIVelocityIepeSensorSensitivityUnits
        {
            MilliVoltsPerG = 10070,
            VoltsPerG = 10071,
            MetersPerSecondSquaredPerG = 14610,
            MillivoltsPerMillimeterPerSecond = 15963,
            MillivoltsPerInchPerSecond = 15964,
        }
        public enum AIExcitationSource
        {
            External = 10167,
            Internal = 10200,
            None = 10230,
        }
        public enum AOVoltageUnits
        {
            // Token: 0x0400050C RID: 1292
            Volts = 10348,
            // Token: 0x0400050D RID: 1293
            FromCustomScale = 10065
        }
        public enum SampleClockActiveEdge
        {
            Rising = 10152,
            Falling = 10171
        }
        public enum SampleQuantityMode
        {
            FiniteSamples = 10178,
            ContinuousSamples = 10123
        }

        public enum FillMode
        {
            GroupByChannel = 0,
            GroupByScanNumber = 1,
        }
        public enum Edge
        {
            Rising = 10280,
            Falling = 10171,
            Either = 10352
        }
        public enum WriteMode
        {
            Auto = 0,
            EntireBuffer = 1
        }
        public enum VoltageUnit
        {

        }
        public enum AIMeasurementType
        {
            Current = 10134,
            Frequency = 10181,
            Resistance = 10278,
            StrainGage = 10300,
            Rtd = 10301,
            Thermistor = 10302,
            Thermocouple = 10303,
            BuiltInTemperatureSensor = 10311,
            //[Display(Name = nameof(QuickSV.Modules.Daq.NI_DAQmx.Properties.Resources.Voltage),
            //    ResourceType = typeof(QuickSV.Modules.Daq.NI_DAQmx.Properties.Resources))]
            Voltage = 10322,
            VoltageCustomWithExcitation = 10323,
            VoltageRms = 10350,
            CurrentRms = 10351,
            Lvdt = 10352,
            Rvdt = 10353,
            Microphone = 10354,
            Accelerometer = 10356,
            TedsSensor = 12531,
            EddyCurrentProximityProbe = 14835,
            ForceIepeSensor = 15895,
            ForceBridge = 15899,
            PressureBridge = 15902,
            TorqueBridge = 15905,
            Bridge = 15908,
            //[Display(Name = nameof(QuickSV.Modules.Daq.NI_DAQmx.Properties.Resources.IEPE),
            //    ResourceType = typeof(QuickSV.Modules.Daq.NI_DAQmx.Properties.Resources))]
            VelocityIepeSensor = 15966,
            RosetteStrainGage = 15980,
            AccelerationCharge = 16104,
            Charge = 16105,
            AccelerationFourWireDCVoltage = 16106,
        }
        public enum AICoupling
        {
            AC = 10045,
            DC = 10050,
            Ground = 10066,
        }
        [Flags]
        public enum CouplingTypes
        {
            None = 0,
            AC = 1,
            DC = 2,
            Ground = 4,
        }
    }
}
