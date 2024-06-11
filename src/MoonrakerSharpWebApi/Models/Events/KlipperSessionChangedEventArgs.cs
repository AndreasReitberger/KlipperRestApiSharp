using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use SessionChangedEventArgs instead")]
    public class KlipperSessionChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperEventSession? Session { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
