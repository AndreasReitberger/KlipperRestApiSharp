using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperGcodeMetaResultChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperGcodeMetaResult? NewResult { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodeMetaResultChangedEventArgs);
        #endregion
    }
}
