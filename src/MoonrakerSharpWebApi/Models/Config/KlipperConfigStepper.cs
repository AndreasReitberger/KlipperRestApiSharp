using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigStepper : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("homing_positive_dir")]
        bool homingPositiveDir;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("homing_retract_dist")]
        long homingRetractDist;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("position_endstop")]
        double positionEndstop;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("full_steps_per_rotation")]
        long fullStepsPerRotation;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("endstop_pin")]
        string endstopPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rotation_distance")]
        long rotationDistance;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gear_ratio")]
        List<object> gearRatio = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("second_homing_speed")]
        double secondHomingSpeed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("step_pin")]
        string stepPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("position_min")]
        long positionMin;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("microsteps")]
        long microsteps;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("homing_speed")]
        double homingSpeed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("position_max")]
        long positionMax;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dir_pin")]
        string dirPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("homing_retract_speed")]
        long homingRetractSpeed;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_pin")]
        string enablePin = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
