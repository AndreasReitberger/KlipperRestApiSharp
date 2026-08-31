using System.Text.Json.Serialization;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigPrinter : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("max_velocity")]
        public partial long MaxVelocity { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_z_velocity")]
        public partial long MaxZVelocity { get; set; }

        [ObservableProperty]
        [JsonPropertyName("kinematics")]
        public partial string Kinematics { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("max_accel")]
        public partial long MaxAccel { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_z_accel")]
        public partial long MaxZAccel { get; set; }

        [ObservableProperty]
        [JsonPropertyName("square_corner_velocity")]
        public partial long SquareCornerVelocity { get; set; }

        [ObservableProperty]
        [JsonPropertyName("move_flush_time")]
        public partial double MoveFlushTime { get; set; }

        [ObservableProperty]
        [JsonPropertyName("buffer_time_start")]
        public partial double BufferTimeStart { get; set; }

        [ObservableProperty]
        [JsonPropertyName("buffer_time_low")]
        public partial long BufferTimeLow { get; set; }

        [ObservableProperty]
        [JsonPropertyName("buffer_time_high")]
        public partial long BufferTimeHigh { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_accel_to_decel")]
        public partial long MaxAccelToDecel { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
