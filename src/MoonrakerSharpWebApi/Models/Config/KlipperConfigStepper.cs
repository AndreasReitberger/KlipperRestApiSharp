using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigStepper : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("homing_positive_dir")]
        bool homingPositiveDir;

        [ObservableProperty]
        [property: JsonProperty("homing_retract_dist")]
        long homingRetractDist;

        [ObservableProperty]
        [property: JsonProperty("position_endstop")]
        double positionEndstop;

        [ObservableProperty]
        [property: JsonProperty("full_steps_per_rotation")]
        long fullStepsPerRotation;

        [ObservableProperty]
        [property: JsonProperty("endstop_pin")]
        string endstopPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("rotation_distance")]
        long rotationDistance;

        [ObservableProperty]
        [property: JsonProperty("gear_ratio")]
        List<object> gearRatio = [];

        [ObservableProperty]
        [property: JsonProperty("second_homing_speed")]
        double secondHomingSpeed;

        [ObservableProperty]
        [property: JsonProperty("step_pin")]
        string stepPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("position_min")]
        long positionMin;

        [ObservableProperty]
        [property: JsonProperty("microsteps")]
        long microsteps;

        [ObservableProperty]
        [property: JsonProperty("homing_speed")]
        double homingSpeed;

        [ObservableProperty]
        [property: JsonProperty("position_max")]
        long positionMax;

        [ObservableProperty]
        [property: JsonProperty("dir_pin")]
        string dirPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("homing_retract_speed")]
        long homingRetractSpeed;

        [ObservableProperty]
        [property: JsonProperty("enable_pin")]
        string enablePin= string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
