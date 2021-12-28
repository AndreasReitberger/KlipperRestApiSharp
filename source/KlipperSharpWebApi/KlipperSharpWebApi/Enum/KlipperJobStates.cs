using System.Runtime.Serialization;

namespace AndreasReitberger.Enum
{
    public enum KlipperJobStates
    {
        [EnumMember(Value = "completed")]
        Completed = 0,
        [EnumMember(Value = "klippy_shutdown")]
        KlippyShutdown = 1,
        [EnumMember(Value = "cancelled")]
        Cancelled = 2,
        [EnumMember(Value = "error")]
        Error = 99,
    }
}
