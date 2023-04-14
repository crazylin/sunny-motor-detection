using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MotorDetection.Algorithm
{
    public enum AverageMode
    {
        NoAverage,
        LinearAverage,
        ExponentAverage,
        PeakHolding,
    }
}
