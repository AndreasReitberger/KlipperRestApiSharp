using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperJobListStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public string NewJobListStatus { get; set; } = string.Empty;
        public string PreviousJobListStatus { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperJobListStateChangedEventArgs);
        #endregion
    }
}
