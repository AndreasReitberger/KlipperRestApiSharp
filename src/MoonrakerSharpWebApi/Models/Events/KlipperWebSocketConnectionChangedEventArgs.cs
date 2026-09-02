using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperWebSocketConnectionChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public long? ConnectionId { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperWebSocketConnectionChangedEventArgs);
        #endregion
    }
}
