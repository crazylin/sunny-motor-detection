using System;
using System.Collections.Generic;
using System.Text;
using MotorDetection.Daq.Models;

namespace MotorDetection.Daq
{
    public class DaqDataArgs : EventArgs
    {
        public BaseDaqSetting DaqSetting { set; get; }
        public BaseAIChannelSetting[] AIChannelSettings { set; get; }
    }
}
