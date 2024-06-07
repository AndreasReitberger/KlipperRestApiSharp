using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use IsPrintingStateChangedEventArgs instead")]
    public class KlipperIsPrintingStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public bool IsPrinting { get; set; }
        public bool IsPaused { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
