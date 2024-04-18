using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_velocity")]
        long maxVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_z_velocity")]
        long maxZVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("kinematics")]
        string kinematics = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_accel")]
        long maxAccel;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_z_accel")]
        long maxZAccel;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("square_corner_velocity")]
        long squareCornerVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("move_flush_time")]
        double moveFlushTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("buffer_time_start")]
        double bufferTimeStart;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("buffer_time_low")]
        long bufferTimeLow;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("buffer_time_high")]
        long bufferTimeHigh;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_accel_to_decel")]
        long maxAccelToDecel;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
