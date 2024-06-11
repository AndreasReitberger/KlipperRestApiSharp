using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use WebsocketEventArgs instead")]
    public class KlipperWebSocketDataEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public byte[]? Data { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
