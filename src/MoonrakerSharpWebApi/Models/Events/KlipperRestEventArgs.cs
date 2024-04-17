using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("use `RestEventArgs` instead")]
    public class KlipperRestEventArgs : EventArgs
    {
        #region Properties
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public Uri? Uri { get; set; }
        public Exception? Exception { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
