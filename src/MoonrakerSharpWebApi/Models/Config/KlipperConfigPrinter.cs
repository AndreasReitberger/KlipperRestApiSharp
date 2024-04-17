using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("max_velocity")]
        long maxVelocity;

        [ObservableProperty]
        [property: JsonProperty("max_z_velocity")]
        long maxZVelocity;

        [ObservableProperty]
        [property: JsonProperty("kinematics")]
        string kinematics = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("max_accel")]
        long maxAccel;

        [ObservableProperty]
        [property: JsonProperty("max_z_accel")]
        long maxZAccel;

        [ObservableProperty]
        [property: JsonProperty("square_corner_velocity")]
        long squareCornerVelocity;

        [ObservableProperty]
        [property: JsonProperty("move_flush_time")]
        double moveFlushTime;

        [ObservableProperty]
        [property: JsonProperty("buffer_time_start")]
        double bufferTimeStart;

        [ObservableProperty]
        [property: JsonProperty("buffer_time_low")]
        long bufferTimeLow;

        [ObservableProperty]
        [property: JsonProperty("buffer_time_high")]
        long bufferTimeHigh;

        [ObservableProperty]
        [property: JsonProperty("max_accel_to_decel")]
        long maxAccelToDecel;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
