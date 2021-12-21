using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperActiveJobStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusJob NewJobState { get; set; }
        public KlipperStatusJob PreviousJobState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
