namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("virtual_sdcard")]
        public partial KlipperConfigVirtualSdcard? VirtualSdcard { get; set; }

        [ObservableProperty]

        [JsonPropertyName("printer")]
        public partial KlipperConfigPrinter? Printer { get; set; }

        [ObservableProperty]

        [JsonPropertyName("pause_resume")]
        public partial KlipperStatusPauseResume? PauseResume { get; set; }

        [ObservableProperty]

        [JsonPropertyName("display_status")]
        public partial KlipperStatusDisplay? DisplayStatus { get; set; }

        [ObservableProperty]

        [JsonPropertyName("stepper_y")]
        public partial KlipperConfigStepper? StepperY { get; set; }

        [ObservableProperty]

        [JsonPropertyName("stepper_x")]
        public partial KlipperConfigStepper? StepperX { get; set; }

        [ObservableProperty]

        [JsonPropertyName("gcode_macro RESUME")]
        public partial KlipperGcodeMacro? GcodeMacroResume { get; set; }

        [ObservableProperty]

        [JsonPropertyName("gcode_macro PAUSE")]
        public partial KlipperGcodeMacro? GcodeMacroPause { get; set; }

        [ObservableProperty]

        [JsonPropertyName("gcode_macro CANCEL_PRINT")]
        public partial KlipperGcodeMacro? GcodeMacroCancelPrint { get; set; }

        [ObservableProperty]

        [JsonPropertyName("fan")]
        public partial KlipperConfigFan? Fan { get; set; }

        [ObservableProperty]

        [JsonPropertyName("stepper_z")]
        public partial KlipperConfigStepper? StepperZ { get; set; }

        [ObservableProperty]

        [JsonPropertyName("mcu")]
        public partial KlipperConfigMcu? Mcu { get; set; }

        [ObservableProperty]

        [JsonPropertyName("display")]
        public partial KlipperConfigDisplay? Display { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extruder")]
        public partial KlipperConfigExtruder? Extruder { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperConfig);
        #endregion
    }
}
