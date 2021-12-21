using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperDisplayStatusChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusDisplay NewDisplayStatus { get; set; }
        public KlipperStatusDisplay PreviousDisplayStatus { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
