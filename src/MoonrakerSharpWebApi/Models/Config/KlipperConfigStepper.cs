using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigStepper : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("homing_positive_dir")]
        public partial bool HomingPositiveDir { get; set; }

        [ObservableProperty]
        [JsonPropertyName("homing_retract_dist")]
        public partial long HomingRetractDist { get; set; }

        [ObservableProperty]
        [JsonPropertyName("position_endstop")]
        public partial double PositionEndstop { get; set; }

        [ObservableProperty]
        [JsonPropertyName("full_steps_per_rotation")]
        public partial long FullStepsPerRotation { get; set; }

        [ObservableProperty]
        [JsonPropertyName("endstop_pin")]
        public partial string EndstopPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("rotation_distance")]
        public partial long RotationDistance { get; set; }

        [ObservableProperty]
        [JsonPropertyName("gear_ratio")]
        public partial List<object> GearRatio { get; set; } = [];

        [ObservableProperty]
        [JsonPropertyName("second_homing_speed")]
        public partial double SecondHomingSpeed { get; set; }

        [ObservableProperty]
        [JsonPropertyName("step_pin")]
        public partial string StepPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("position_min")]
        public partial long PositionMin { get; set; }

        [ObservableProperty]
        [JsonPropertyName("microsteps")]
        public partial long Microsteps { get; set; }

        [ObservableProperty]
        [JsonPropertyName("homing_speed")]
        public partial double HomingSpeed { get; set; }

        [ObservableProperty]
        [JsonPropertyName("position_max")]
        public partial long PositionMax { get; set; }

        [ObservableProperty]
        [JsonPropertyName("dir_pin")]
        public partial string DirPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("homing_retract_speed")]
        public partial long HomingRetractSpeed { get; set; }

        [ObservableProperty]
        [JsonPropertyName("enable_pin")]
        public partial string EnablePin { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperConfigStepper);
        #endregion
    }
}
