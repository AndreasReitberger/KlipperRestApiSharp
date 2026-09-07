using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperVirtualSdCardStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusVirtualSdcard? NewState { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperVirtualSdCardStateChangedEventArgs);
        #endregion
    }
}
