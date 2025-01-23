using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("max_velocity")]
        public partial long MaxVelocity { get; set; }

        [ObservableProperty]

        [JsonProperty("max_z_velocity")]
        public partial long MaxZVelocity { get; set; }

        [ObservableProperty]

        [JsonProperty("kinematics")]
        public partial string Kinematics { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("max_accel")]
        public partial long MaxAccel { get; set; }

        [ObservableProperty]

        [JsonProperty("max_z_accel")]
        public partial long MaxZAccel { get; set; }

        [ObservableProperty]

        [JsonProperty("square_corner_velocity")]
        public partial long SquareCornerVelocity { get; set; }

        [ObservableProperty]

        [JsonProperty("move_flush_time")]
        public partial double MoveFlushTime { get; set; }

        [ObservableProperty]

        [JsonProperty("buffer_time_start")]
        public partial double BufferTimeStart { get; set; }

        [ObservableProperty]

        [JsonProperty("buffer_time_low")]
        public partial long BufferTimeLow { get; set; }

        [ObservableProperty]

        [JsonProperty("buffer_time_high")]
        public partial long BufferTimeHigh { get; set; }

        [ObservableProperty]

        [JsonProperty("max_accel_to_decel")]
        public partial long MaxAccelToDecel { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
