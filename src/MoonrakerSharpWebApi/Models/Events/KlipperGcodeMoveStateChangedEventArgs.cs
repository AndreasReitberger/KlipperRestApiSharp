using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperGcodeMoveStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusGcodeMove? NewState { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodeMoveStateChangedEventArgs);
        #endregion
    }
}
