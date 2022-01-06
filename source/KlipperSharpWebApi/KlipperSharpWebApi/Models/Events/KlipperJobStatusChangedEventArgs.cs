using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperJobStatusChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusJob NewJobStatus { get; set; }
        public KlipperStatusJob PreviousJobStatus { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
