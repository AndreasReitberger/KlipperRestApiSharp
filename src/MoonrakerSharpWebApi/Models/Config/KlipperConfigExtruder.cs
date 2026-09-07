using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("control")]
        public partial string Control { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("pid_kp")]
        public partial string PidKp { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("sensor_type")]
        public partial string SensorType { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("sensor_pin")]
        public partial string SensorPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("nozzle_diameter")]
        public partial string NozzleDiameter { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("rotation_distance")]
        public partial string RotationDistance { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("heater_pin")]
        public partial string HeaterPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("step_pin")]
        public partial string StepPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("min_temp")]
        public partial long MinTemp { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pid_kd")]
        public partial string PidKd { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("microsteps")]
        public partial long Microsteps { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pid_ki")]
        public partial string PidKi { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("filament_diameter")]
        public partial string FilamentDiameter { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("dir_pin")]
        public partial string DirPin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("max_temp")]
        public partial long MaxTemp { get; set; }

        [ObservableProperty]
        [JsonPropertyName("enable_pin")]
        public partial string EnablePin { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("pullup_resistor")]
        public partial long PullupResistor { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_extrude_only_velocity")]
        public partial double MaxExtrudeOnlyVelocity { get; set; }

        [ObservableProperty]
        [JsonPropertyName("gear_ratio")]
        public partial List<object> GearRatio { get; set; } = [];

        [ObservableProperty]
        [JsonPropertyName("max_extrude_only_distance")]
        public partial long MaxExtrudeOnlyDistance { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pressure_advance")]
        public partial double PressureAdvance { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_extrude_cross_section")]
        public partial double MaxExtrudeCrossSection { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pwm_cycle_time")]
        public partial double PwmCycleTime { get; set; }

        [ObservableProperty]
        [JsonPropertyName("instantaneous_corner_velocity")]
        public partial long InstantaneousCornerVelocity { get; set; }

        [ObservableProperty]
        [JsonPropertyName("full_steps_per_rotation")]
        public partial long FullStepsPerRotation { get; set; }

        [ObservableProperty]
        [JsonPropertyName("pressure_advance_smooth_time")]
        public partial double PressureAdvanceSmoothTime { get; set; }

        [ObservableProperty]
        [JsonPropertyName("smooth_time")]
        public partial long SmoothTime { get; set; }

        [ObservableProperty]
        [JsonPropertyName("inline_resistor")]
        public partial long InlineResistor { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_power")]
        public partial long MaxPower { get; set; }

        [ObservableProperty]
        [JsonPropertyName("min_extrude_temp")]
        public partial long MinExtrudeTemp { get; set; }

        [ObservableProperty]
        [JsonPropertyName("max_extrude_only_accel")]
        public partial double MaxExtrudeOnlyAccel { get; set; }

        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperConfigExtruder);
        #endregion
    }
}
