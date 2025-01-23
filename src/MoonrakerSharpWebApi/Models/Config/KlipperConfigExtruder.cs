using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("control")]
        public partial string Control { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("pid_kp")]
        public partial string PidKp { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("sensor_type")]
        public partial string SensorType { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("sensor_pin")]
        public partial string SensorPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("nozzle_diameter")]
        public partial string NozzleDiameter { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("rotation_distance")]
        public partial string RotationDistance { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("heater_pin")]
        public partial string HeaterPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("step_pin")]
        public partial string StepPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("min_temp")]
        public partial long MinTemp { get; set; }

        [ObservableProperty]

        [JsonProperty("pid_kd")]
        public partial string PidKd { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("microsteps")]
        public partial long Microsteps { get; set; }

        [ObservableProperty]

        [JsonProperty("pid_ki")]
        public partial string PidKi { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("filament_diameter")]
        public partial string FilamentDiameter { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("dir_pin")]
        public partial string DirPin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("max_temp")]
        public partial long MaxTemp { get; set; }

        [ObservableProperty]

        [JsonProperty("enable_pin")]
        public partial string EnablePin { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("pullup_resistor")]
        public partial long PullupResistor { get; set; }

        [ObservableProperty]

        [JsonProperty("max_extrude_only_velocity")]
        public partial double MaxExtrudeOnlyVelocity { get; set; }

        [ObservableProperty]

        [JsonProperty("gear_ratio")]
        public partial List<object> GearRatio { get; set; } = [];

        [ObservableProperty]

        [JsonProperty("max_extrude_only_distance")]
        public partial long MaxExtrudeOnlyDistance { get; set; }

        [ObservableProperty]

        [JsonProperty("pressure_advance")]
        public partial double PressureAdvance { get; set; }

        [ObservableProperty]

        [JsonProperty("max_extrude_cross_section")]
        public partial double MaxExtrudeCrossSection { get; set; }

        [ObservableProperty]

        [JsonProperty("pwm_cycle_time")]
        public partial double PwmCycleTime { get; set; }

        [ObservableProperty]

        [JsonProperty("instantaneous_corner_velocity")]
        public partial long InstantaneousCornerVelocity { get; set; }

        [ObservableProperty]

        [JsonProperty("full_steps_per_rotation")]
        public partial long FullStepsPerRotation { get; set; }

        [ObservableProperty]

        [JsonProperty("pressure_advance_smooth_time")]
        public partial double PressureAdvanceSmoothTime { get; set; }

        [ObservableProperty]

        [JsonProperty("smooth_time")]
        public partial long SmoothTime { get; set; }

        [ObservableProperty]

        [JsonProperty("inline_resistor")]
        public partial long InlineResistor { get; set; }

        [ObservableProperty]

        [JsonProperty("max_power")]
        public partial long MaxPower { get; set; }

        [ObservableProperty]

        [JsonProperty("min_extrude_temp")]
        public partial long MinExtrudeTemp { get; set; }

        [ObservableProperty]

        [JsonProperty("max_extrude_only_accel")]
        public partial double MaxExtrudeOnlyAccel { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
