using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use `ListeningChangedEventArgs` instead")]
    public class KlipperEventListeningChangedEventArgs : KlipperSessionChangedEventArgs
    {
        #region Properties
        public bool IsListening { get; set; } = false;
        public bool IsListeningToWebSocket { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
