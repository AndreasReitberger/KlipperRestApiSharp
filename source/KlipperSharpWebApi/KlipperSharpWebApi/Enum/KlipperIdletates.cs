using System.Runtime.Serialization;

namespace AndreasReitberger.Enum
{
    public enum KlipperIdletates
    {
        [EnumMember(Value = "Idle")]
        Idle = 0,
        [EnumMember(Value = "Ready")]
        Ready = 1,
        [EnumMember(Value = "Printing")]
        Printing = 2,
    }
}
