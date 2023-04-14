using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDetection.Algorithm
{
    public enum SpectrumType
    {
        /// <summary>
        /// 幅值谱
        /// </summary>
        Amplitude,
        /// <summary>
        /// 均方根值谱
        /// </summary>
        Rms,
        /// <summary>
        /// 功率谱
        /// </summary>
        AutoPower,
        /// <summary>
        /// 功率谱密度
        /// </summary>
        AutoPowerDensity,
    }
}
