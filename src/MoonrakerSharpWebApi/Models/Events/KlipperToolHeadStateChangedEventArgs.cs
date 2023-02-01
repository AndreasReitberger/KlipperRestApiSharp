using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperToolHeadStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusToolhead NewToolheadState { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
