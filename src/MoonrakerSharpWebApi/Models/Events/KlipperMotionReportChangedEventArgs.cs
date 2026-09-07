using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperMotionReportChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusMotionReport? NewState { get; set; }
        public KlipperStatusMotionReport? PreviousState { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperMotionReportChangedEventArgs);
        #endregion
    }
}
