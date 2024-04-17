using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperActiveJobStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public string NewJobState { get; set; } = string.Empty;
        //public KlipperStatusJob NewJobState { get; set; }
        public string PreviousJobState { get; set; } = string.Empty;
        //public KlipperStatusJob PreviousJobState { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
