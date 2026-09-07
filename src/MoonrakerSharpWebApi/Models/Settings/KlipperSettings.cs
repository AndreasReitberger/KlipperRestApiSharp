namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("virtual_sdcard")]
        public partial KlipperConfigVirtualSdcard? VirtualSdcard { get; set; }

        [ObservableProperty]

        [JsonPropertyName("printer")]
        public partial KlipperConfigPrinter? Printer { get; set; }

        [ObservableProperty]

        [JsonPropertyName("verify_heater extruder")]
        public partial KlipperSettingVerifyHeaterExtruder? VerifyHeaterExtruder { get; set; }

        [ObservableProperty]

        [JsonPropertyName("force_move")]
        public partial KlipperSettingForceMove? ForceMove { get; set; }

        [ObservableProperty]

        [JsonPropertyName("pause_resume")]
        public partial KlipperSettingPauseResume? PauseResume { get; set; }

        [ObservableProperty]

        [JsonPropertyName("stepper_z")]
        public partial KlipperConfigStepper? StepperZ { get; set; }

        [ObservableProperty]

        [JsonPropertyName("stepper_y")]
        public partial KlipperConfigStepper? StepperY { get; set; }

        [ObservableProperty]

        [JsonPropertyName("stepper_x")]
        public partial KlipperConfigStepper? StepperX { get; set; }

        [ObservableProperty]

        [JsonPropertyName("idle_timeout")]
        public partial KlipperSettingIdleTimeout? IdleTimeout { get; set; }

        [ObservableProperty]

        [JsonPropertyName("fan")]
        public partial KlipperConfigFan? Fan { get; set; }

        [ObservableProperty]

        [JsonPropertyName("mcu")]
        public partial KlipperConfigMcu? Mcu { get; set; }

        [ObservableProperty]

        [JsonPropertyName("display")]
        public partial KlipperSettingsDisplay? Display { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extruder")]
        public partial KlipperConfigExtruder? Extruder { get; set; }

        [ObservableProperty]

        [JsonPropertyName("heater_bed")]
        public partial KlipperConfigHeaterBed? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperSettings);
        #endregion
    }
}
