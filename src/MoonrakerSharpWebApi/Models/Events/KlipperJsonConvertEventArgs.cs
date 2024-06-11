using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use JsonConvertEventArgs instead")]
    public class KlipperJsonConvertEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; } = string.Empty;
        public string OriginalString { get; set; } = string.Empty;
        public string TargetType { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
