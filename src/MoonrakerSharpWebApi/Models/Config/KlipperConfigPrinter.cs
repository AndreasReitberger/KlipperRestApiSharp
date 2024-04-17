using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigPrinter
    {
        #region Properties
        [JsonProperty("max_velocity")]
        public long MaxVelocity { get; set; }

        [JsonProperty("max_z_velocity")]
        public long MaxZVelocity { get; set; }

        [JsonProperty("kinematics")]
        public string Kinematics { get; set; } = string.Empty;

        [JsonProperty("max_accel")]
        public long MaxAccel { get; set; }

        [JsonProperty("max_z_accel")]
        public long MaxZAccel { get; set; }
        [JsonProperty("square_corner_velocity")]
        public long SquareCornerVelocity { get; set; }

        [JsonProperty("move_flush_time")]
        public double MoveFlushTime { get; set; }

        [JsonProperty("buffer_time_start")]
        public double BufferTimeStart { get; set; }

        [JsonProperty("buffer_time_low")]
        public long BufferTimeLow { get; set; }

        [JsonProperty("buffer_time_high")]
        public long BufferTimeHigh { get; set; }

        [JsonProperty("max_accel_to_decel")]
        public long MaxAccelToDecel { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
