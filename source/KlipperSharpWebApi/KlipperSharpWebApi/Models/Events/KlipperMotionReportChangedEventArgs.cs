using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public class KlipperMotionReportChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusMotionReport NewState { get; set; }
        public KlipperStatusMotionReport PreviousState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
