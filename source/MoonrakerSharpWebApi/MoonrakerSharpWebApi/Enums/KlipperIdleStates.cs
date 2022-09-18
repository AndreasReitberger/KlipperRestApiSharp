using System.Runtime.Serialization;

namespace AndreasReitberger.API.Moonraker.Enum
{
    public enum KlipperIdleStates
    {
        [EnumMember(Value = "Idle")]
        Idle = 0,
        [EnumMember(Value = "Ready")]
        Ready = 1,
        [EnumMember(Value = "Printing")]
        Printing = 2,
        [EnumMember(Value = "Shutdown")]
        Shutdown = 2,
    }
}
