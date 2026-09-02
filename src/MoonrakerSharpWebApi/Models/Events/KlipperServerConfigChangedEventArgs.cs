using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperServerConfigChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperServerConfig? NewConfiguration { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperServerConfigChangedEventArgs);
        #endregion
    }
}
