using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigStepper
    {
        #region Properties
        [JsonProperty("homing_positive_dir")]
        public bool HomingPositiveDir { get; set; }

        [JsonProperty("homing_retract_dist")]
        public long HomingRetractDist { get; set; }

        [JsonProperty("position_endstop")]
        public double PositionEndstop { get; set; }

        [JsonProperty("full_steps_per_rotation")]
        public long FullStepsPerRotation { get; set; }

        [JsonProperty("endstop_pin")]
        public string EndstopPin { get; set; } = string.Empty;

        [JsonProperty("rotation_distance")]
        public long RotationDistance { get; set; }

        [JsonProperty("gear_ratio")]
        public List<object> GearRatio { get; set; } = [];

        [JsonProperty("second_homing_speed")]
        public double SecondHomingSpeed { get; set; }

        [JsonProperty("step_pin")]
        public string StepPin { get; set; } = string.Empty;

        [JsonProperty("position_min")]
        public long PositionMin { get; set; }

        [JsonProperty("microsteps")]
        public long Microsteps { get; set; }

        [JsonProperty("homing_speed")]
        public double HomingSpeed { get; set; }

        [JsonProperty("position_max")]
        public long PositionMax { get; set; }

        [JsonProperty("dir_pin")]
        public string DirPin { get; set; } = string.Empty;

        [JsonProperty("homing_retract_speed")]
        public long HomingRetractSpeed { get; set; }

        [JsonProperty("enable_pin")]
        public string EnablePin { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
