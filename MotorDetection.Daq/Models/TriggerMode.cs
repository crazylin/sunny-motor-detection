using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDetection.Daq.Models
{
    public enum TriggerMode
    {
        Absolute,
        RisingEdge,
        DescentEdge
    }

    public enum TriggerType
    {
        Software,
        Hardware,
    }
}
