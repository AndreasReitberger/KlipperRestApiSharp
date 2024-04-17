using AndreasReitberger.API.Print3dServer.Core.Events;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperMotionReportChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusMotionReport? NewState { get; set; }
        public KlipperStatusMotionReport? PreviousState { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
