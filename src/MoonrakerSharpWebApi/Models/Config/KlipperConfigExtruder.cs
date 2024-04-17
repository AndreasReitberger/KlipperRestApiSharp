using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("control")]
        string control = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("pid_kp")]
        string pidKp = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("sensor_type")]
        string sensorType = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("sensor_pin")]
        string sensorPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("nozzle_diameter")]
        string nozzleDiameter = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("rotation_distance")]
        string rotationDistance = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("heater_pin")]
        string heaterPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("step_pin")]
        string stepPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("min_temp")]
        long minTemp;

        [ObservableProperty]
        [property: JsonProperty("pid_kd")]
        string pidKd = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("microsteps")]
        long microsteps;

        [ObservableProperty]
        [property: JsonProperty("pid_ki")]
        string pidKi = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("filament_diameter")]
        string filamentDiameter = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("dir_pin")]
        string dirPin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("max_temp")]
        long maxTemp;

        [ObservableProperty]
        [property: JsonProperty("enable_pin")]
        string enablePin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("pullup_resistor")]
        long pullupResistor;

        [ObservableProperty]
        [property: JsonProperty("max_extrude_only_velocity")]
        double maxExtrudeOnlyVelocity;

        [ObservableProperty]
        [property: JsonProperty("gear_ratio")]
        List<object> gearRatio = [];

        [ObservableProperty]
        [property: JsonProperty("max_extrude_only_distance")]
        long maxExtrudeOnlyDistance;

        [ObservableProperty]
        [property: JsonProperty("pressure_advance")]
        double pressureAdvance;

        [ObservableProperty]
        [property: JsonProperty("max_extrude_cross_section")]
        double maxExtrudeCrossSection;

        [ObservableProperty]
        [property: JsonProperty("pwm_cycle_time")]
        double pwmCycleTime;

        [ObservableProperty]
        [property: JsonProperty("instantaneous_corner_velocity")]
        long instantaneousCornerVelocity;

        [ObservableProperty]
        [property: JsonProperty("full_steps_per_rotation")]
        long fullStepsPerRotation;

        [ObservableProperty]
        [property: JsonProperty("pressure_advance_smooth_time")]
        double pressureAdvanceSmoothTime;

        [ObservableProperty]
        [property: JsonProperty("smooth_time")]
        long smoothTime;

        [ObservableProperty]
        [property: JsonProperty("inline_resistor")]
        long inlineResistor;

        [ObservableProperty]
        [property: JsonProperty("max_power")]
        long maxPower;

        [ObservableProperty]
        [property: JsonProperty("min_extrude_temp")]
        long minExtrudeTemp;

        [ObservableProperty]
        [property: JsonProperty("max_extrude_only_accel")]
        double maxExtrudeOnlyAccel;

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
