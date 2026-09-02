using AndreasReitberger.API.Print3dServer.Core.Events;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperGcodeCacheChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public List<KlipperGcode> CachedGcodes { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperGcodeCacheChangedEventArgs);
        #endregion
    }
}
