using System.Runtime.Serialization;

namespace AndreasReitberger.Enum
{
    public enum KlipperPrintStates
    {
        [EnumMember(Value = "standby")]
        Standby = 0,
        [EnumMember(Value = "printing")]
        Printing = 1,
        [EnumMember(Value = "paused")]
        Paused = 2,
        [EnumMember(Value = "complete")]
        Complete = 3,
        [EnumMember(Value = "cancelled")]
        Cancelled = 4,
        [EnumMember(Value = "error")]
        Error = 99,
    }
}
