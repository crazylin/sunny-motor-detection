using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;
using Caliburn.Micro;
using MotorDetection.Daq.Models;
using Newtonsoft.Json;
using MotorDetection.Daq.NI_DAQ;



namespace MotorDetection.Daq.NI_DAQ.Models
{
    public class NiAIChannelSetting : BaseAIChannelSetting
    {
        public NativeMethods.AIMeasurementType AIMeasurementType { set; get; } = NativeMethods.AIMeasurementType.Voltage;
        public NativeMethods.AICoupling AICoupling { set; get; } = NativeMethods.AICoupling.DC;
        public string VoltageRng { set; get; }

        [JsonIgnore] public BindableCollection<NativeMethods.AIMeasurementType> AIMeasurementTypes { set; get; } = new BindableCollection<NativeMethods.AIMeasurementType>();
        [JsonIgnore] public BindableCollection<NativeMethods.AICoupling> AICouplings { set; get; } = new BindableCollection<NativeMethods.AICoupling>();

        [JsonIgnore] public BindableCollection<string> VoltageRngs { set; get; } = new BindableCollection<string>();

        [JsonIgnore] public BindableCollection<string> MeasurementUnits { set; get; } = new BindableCollection<string>();
        public static IEnumerable<string> GetMeasurementUnitName()
        {
            var list = new List<string>();
            list.Add("m");
            list.Add("m/s");
            list.Add("m/s²");
            list.Add("mm");
            list.Add("mm/s");
            list.Add("mm/s²");
            list.Add("μm");
            list.Add("μm/s");
            list.Add("μm/s²");
            list.Add("nm");
            list.Add("nm/s");
            list.Add("nm/s²");
            list.Add("g");
            list.Add("gal");
            list.Add("kg");
            list.Add("N");
            list.Add("kN");
            list.Add("T");
            list.Add("mV");
            list.Add("V");
            list.Add("mA");
            list.Add("A");
            list.Add("Pa");
            list.Add("μPa");
            list.Add("mPa");
            list.Add("bar");
            list.Add("℃");
            list.Add("με");
            list.Add("°");
            list.Add("′");
            list.Add("″");
            list.Add("°/s");
            list.Add("′/s");
            list.Add("″/s");
            list.Add("deg");
            list.Add("rad");
            return list;
        }

        public static IEnumerable<string> GetVelocityMeasurementUnitName()
        {
            var list = new List<string>();
            list.Add("pm/s");
            list.Add("nm/s");
            list.Add("μm/s");
            list.Add("mm/s");
            list.Add("m/s");
            return list;
        }

        public static IEnumerable<string> GetDisplacementMeasurementUnitName()
        {
            var list = new List<string>();
            list.Add("pm");
            list.Add("nm");
            list.Add("μm");
            list.Add("mm");
            list.Add("m");
            return list;
        }

        public static string GetMeasurementTypeName(string unit)
        {
            switch (unit.ToLower())
            {
                case "nm":
                case "μm":
                case "mm":
                case "m":
                    return "位移";//QuickSV.Modules.Daq.NI_DAQmx.Properties.Resources.Displacement;
                case "nm/s":
                case "μm/s":
                case "mm/s":
                case "m/s":
                    return "速度";//QuickSV.Modules.Daq.NI_DAQmx.Properties.Resources.Velocity;
                case "nm/s²":
                case "μm/s²":
                case "mm/s²":
                case "m/s²":
                case "g":
                    return "加速度";//QuickSV.Modules.Daq.NI_DAQmx.Properties.Resources.Acceleration;
                default:
                    return string.Empty;
            }
        }

        public static List<string> GetMeasurementUntNameList(string unit)
        {
            string header = "", tail = "";
            var index = unit.IndexOf('/');
            if (index < 0)
                index = unit.IndexOf('*');
            if (index < 0)
            {
                header = unit;
            }
            else
            {
                header = unit.Substring(0, index);
                tail = unit.Substring(index, unit.Length - index);
            }

            var unitList = new List<string>();
            if (header == "g")
            {
                unitList.Add("nm/s²");
                unitList.Add("μm/s²");
                unitList.Add("mm/s²");
                unitList.Add("m/s²");
                unitList.Add("g");
            }
            else
            {
                unitList.Add("nm" + tail);
                unitList.Add("μm" + tail);
                unitList.Add("mm" + tail);
                unitList.Add("m" + tail);
                if (tail == "/s²")
                    unitList.Add("g");
            }


            return unitList;
        }
    }
}
