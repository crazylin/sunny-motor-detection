using System;
using System.Collections.Generic;
using System.Text;
using Caliburn.Micro;
using Newtonsoft.Json;
using OxyPlot;

namespace MotorDetection.Daq.Models
{
    public class BaseAIChannelSetting : Gemini.Modules.Utils.PropertyChangedBase
    {
        private bool _isShow;
        public int Id { set; get; }
        public string Name { set; get; }

        public string ShowName { set; get; }
        public bool IsOn { set; get; }
        public string MeasurementUnit { set; get; }
        public string TestPointName { set; get; }
        public string Description { set; get; }
        public double Calibration { set; get; }

        public bool IsVirtual { set; get; } = false;


        [JsonIgnore] public double[] AvgTimeData { set; get; }

        [JsonIgnore] public double[] TimeData { set; get; }
        [JsonIgnore] public int TimeDataIndex { set; get; }
        [JsonIgnore] public IPlotModel SignalTimePlot { set; get; }
        [JsonIgnore] public double[] FreqData { set; get; }

        [JsonIgnore] public IPlotModel SignalFreqPlot { set; get; }


        [JsonIgnore] public double[] TempTimeData { set; get; }
        [JsonIgnore] public int TempTimeDataIndex { set; get; }

    }
}
