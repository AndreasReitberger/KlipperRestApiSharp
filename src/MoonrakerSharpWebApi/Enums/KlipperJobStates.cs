using System.Runtime.Serialization;

namespace AndreasReitberger.API.Moonraker.Enum
{
    public enum KlipperJobStates
    {
        [EnumMember(Value = "completed")]
        Completed = 0,
        [EnumMember(Value = "klippy_shutdown")]
        KlippyShutdown = 1,
        [EnumMember(Value = "klippy_disconnect")]
        KlippyDisconnect = 2,
        [EnumMember(Value = "in_progress")]
        InProgress = 3,
        [EnumMember(Value = "cancelled")]
        Cancelled = 4,
        [EnumMember(Value = "interrupted")]
        Interrupted = 5,
        [EnumMember(Value = "server_exit")]
        ServerExit = 6,
        [EnumMember(Value = "error")]
        Error = 99,
    }
}
