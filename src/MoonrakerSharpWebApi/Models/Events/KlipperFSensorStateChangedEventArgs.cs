using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public class KlipperFSensorStateChangedEventArgs : KlipperEventArgs
    {
        #region Properties
        public KlipperStatusFilamentSensor NewFSensorState { get; set; }
        public KlipperStatusFilamentSensor PreviousFSensorState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
