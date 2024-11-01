using System.Runtime.Serialization;

namespace AndreasReitberger.API.Moonraker.Enum
{
    public enum KlipperJobStates
    {
        [EnumMember(Value = "completed")]
        Completed = 0,
        [EnumMember(Value = "klippy_shutdown")]
        KlippyShutdown = 1,
        [EnumMember(Value = "in_progress")]
        InProgress = 2,
        [EnumMember(Value = "cancelled")]
        Cancelled = 3,
        [EnumMember(Value = "interrupted")]
        Interrupted = 4,
        [EnumMember(Value = "error")]
        Error = 99,
    }
}
