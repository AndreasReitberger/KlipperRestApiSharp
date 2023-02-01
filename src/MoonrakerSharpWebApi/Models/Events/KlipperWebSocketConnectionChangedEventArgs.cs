using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperWebSocketConnectionChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public long? ConnectionId { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
