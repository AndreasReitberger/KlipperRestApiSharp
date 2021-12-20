using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperEventListeningChangedEventArgs : KlipperSessionChangedEventArgs
    {
        #region Properties
        public bool IsListening { get; set; } = false;
        public bool IsListeningToWebSocket { get; set; } = false;
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
