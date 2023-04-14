using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MotorDetection.Daq.Models;


namespace MotorDetection.Daq
{
    public interface IDaqCard
    {
        event EventHandler<DaqDataArgs> DataReceived;
        bool StartAiTask(BaseDaqSetting daqSetting,double sampleTime = -1);
        bool StartAoTask(BaseDaqSetting daqSetting);
        void StopAiTask();
        void StopAoTask();
        bool GetData(int sampleNumber, BaseAIChannelSetting[] channels,ref bool triggered,out int actSampleNumber, CancellationToken cancellationToken);
    }
}
