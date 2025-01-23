using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigStepper : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("homing_positive_dir")]
        public partial bool HomingPositiveDir { get; set; }

        [ObservableProperty]

        [JsonProperty("homing_retract_dist")]
        public partial long HomingRetractDist { get; set; }

        [ObservableProperty]

        [JsonProperty("position_endstop")]
        public partial double PositionEndstop { get; set; }

        [ObservableProperty]

        [JsonProperty("full_steps_per_rotation")]
        public partial long FullStepsPerRotation { get; set; }

        [ObservableProperty]

        [JsonProperty("endstop_pin")]
        public partial string EndstopPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("rotation_distance")]
        public partial long RotationDistance { get; set; }

        [ObservableProperty]

        [JsonProperty("gear_ratio")]
        public partial List<object> GearRatio { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("second_homing_speed")]
        public partial double SecondHomingSpeed { get; set; }

        [ObservableProperty]

        [JsonProperty("step_pin")]
        public partial string StepPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("position_min")]
        public partial long PositionMin { get; set; }

        [ObservableProperty]

        [JsonProperty("microsteps")]
        public partial long Microsteps { get; set; }

        [ObservableProperty]

        [JsonProperty("homing_speed")]
        public partial double HomingSpeed { get; set; }

        [ObservableProperty]

        [JsonProperty("position_max")]
        public partial long PositionMax { get; set; }

        [ObservableProperty]

        [JsonProperty("dir_pin")]
        public partial string DirPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("homing_retract_speed")]
        public partial long HomingRetractSpeed { get; set; }

        [ObservableProperty]

        [JsonProperty("enable_pin")]
        public partial string EnablePin { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
