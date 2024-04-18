using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("control")]
        string control = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pid_kp")]
        string pidKp = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sensor_type")]
        string sensorType = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sensor_pin")]
        string sensorPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("nozzle_diameter")]
        string nozzleDiameter = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rotation_distance")]
        string rotationDistance = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heater_pin")]
        string heaterPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("step_pin")]
        string stepPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("min_temp")]
        long minTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pid_kd")]
        string pidKd = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("microsteps")]
        long microsteps;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pid_ki")]
        string pidKi = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament_diameter")]
        string filamentDiameter = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("dir_pin")]
        string dirPin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_temp")]
        long maxTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_pin")]
        string enablePin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pullup_resistor")]
        long pullupResistor;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_extrude_only_velocity")]
        double maxExtrudeOnlyVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gear_ratio")]
        List<object> gearRatio = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_extrude_only_distance")]
        long maxExtrudeOnlyDistance;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pressure_advance")]
        double pressureAdvance;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_extrude_cross_section")]
        double maxExtrudeCrossSection;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pwm_cycle_time")]
        double pwmCycleTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("instantaneous_corner_velocity")]
        long instantaneousCornerVelocity;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("full_steps_per_rotation")]
        long fullStepsPerRotation;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pressure_advance_smooth_time")]
        double pressureAdvanceSmoothTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("smooth_time")]
        long smoothTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("inline_resistor")]
        long inlineResistor;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_power")]
        long maxPower;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("min_extrude_temp")]
        long minExtrudeTemp;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_extrude_only_accel")]
        double maxExtrudeOnlyAccel;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
