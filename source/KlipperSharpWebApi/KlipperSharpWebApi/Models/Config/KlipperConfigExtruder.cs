using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperConfigExtruder
    {
        #region Properties
        [JsonProperty("control")]
        public string Control { get; set; }

        [JsonProperty("pid_kp")]
        public string PidKp { get; set; }

        [JsonProperty("sensor_type")]
        public string SensorType { get; set; }

        [JsonProperty("sensor_pin")]
        public string SensorPin { get; set; }

        [JsonProperty("nozzle_diameter")]
        public string NozzleDiameter { get; set; }

        [JsonProperty("rotation_distance")]
        public string RotationDistance { get; set; }

        [JsonProperty("heater_pin")]
        public string HeaterPin { get; set; }

        [JsonProperty("step_pin")]
        public string StepPin { get; set; }

        [JsonProperty("min_temp")]
        public long MinTemp { get; set; }

        [JsonProperty("pid_kd")]
        public string PidKd { get; set; }

        [JsonProperty("microsteps")]
        public long Microsteps { get; set; }

        [JsonProperty("pid_ki")]
        public string PidKi { get; set; }

        [JsonProperty("filament_diameter")]
        public string FilamentDiameter { get; set; }

        [JsonProperty("dir_pin")]
        public string DirPin { get; set; }

        [JsonProperty("max_temp")]
        public long MaxTemp { get; set; }

        [JsonProperty("enable_pin")]
        public string EnablePin { get; set; }

        [JsonProperty("pullup_resistor")]
        public long PullupResistor { get; set; }

        [JsonProperty("max_extrude_only_velocity")]
        public double MaxExtrudeOnlyVelocity { get; set; }

        [JsonProperty("gear_ratio")]
        public List<object> GearRatio { get; set; }

        [JsonProperty("max_extrude_only_distance")]
        public long MaxExtrudeOnlyDistance { get; set; }

        [JsonProperty("pressure_advance")]
        public double PressureAdvance { get; set; }

        [JsonProperty("max_extrude_cross_section")]
        public double MaxExtrudeCrossSection { get; set; }

        [JsonProperty("pwm_cycle_time")]
        public double PwmCycleTime { get; set; }

        [JsonProperty("instantaneous_corner_velocity")]
        public long InstantaneousCornerVelocity { get; set; }

        [JsonProperty("full_steps_per_rotation")]
        public long FullStepsPerRotation { get; set; }

        [JsonProperty("pressure_advance_smooth_time")]
        public double PressureAdvanceSmoothTime { get; set; }

        [JsonProperty("smooth_time")]
        public long SmoothTime { get; set; }

        [JsonProperty("inline_resistor")]
        public long InlineResistor { get; set; }

        [JsonProperty("max_power")]
        public long MaxPower { get; set; }

        [JsonProperty("min_extrude_temp")]
        public long MinExtrudeTemp { get; set; }

        [JsonProperty("max_extrude_only_accel")]
        public double MaxExtrudeOnlyAccel { get; set; }

        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
