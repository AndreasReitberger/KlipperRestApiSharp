using AndreasReitberger.API.Print3dServer.Core.Events;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperFSensorStateChangedEventArgs : Print3dBaseEventArgs
    {
        #region Properties
        public KlipperStatusFilamentSensor? NewFSensorState { get; set; }
        public KlipperStatusFilamentSensor? PreviousFSensorState { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperFSensorStateChangedEventArgs);
        #endregion
    }
}
