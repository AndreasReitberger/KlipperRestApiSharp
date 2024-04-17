using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    [Obsolete("Use `JobStatusChangedEventArgs` changed instead")]
    public class KlipperJobStatusChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusJob? NewJobStatus { get; set; }
        public KlipperStatusJob? PreviousJobStatus { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
