using SerialPortHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortHelper
{
    public static class WorkflowExtension
    {
        public static ProcessTypes GetDetectionType(this WorkflowDescriptor descriptor, string name)
        {
            if (descriptor.DirectiveforMove.Any(o => o.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return ProcessTypes.Movefeedback;

            if (descriptor.DirectiveforSoundPower.Any(o => o.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return ProcessTypes.Powerfeedback;

            return ProcessTypes.None;
        }
        public static DirectiveTypes DirectiveTypeMapping(this WorkflowDescriptor descriptor,string name)
        {
            if (descriptor.DirectiveforMove.Any(o => o.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return DirectiveTypes.Move;

            if (descriptor.DirectiveforSoundPower.Any(o => o.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return DirectiveTypes.SoundOnOff;
            if (descriptor.DirectiveforMonitorPower.Any(o => o.Equals(name, StringComparison.OrdinalIgnoreCase)))
                return DirectiveTypes.MonitorOnOff;
            return DirectiveTypes.Unknow;
        }
    }
}
