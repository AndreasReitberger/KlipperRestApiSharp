using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperFanStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusFan NewFanState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
