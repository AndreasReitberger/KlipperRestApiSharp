using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("virtual_sdcard")]
        public partial KlipperConfigVirtualSdcard? VirtualSdcard { get; set; }

        [ObservableProperty]
        
        [JsonProperty("printer")]
        public partial KlipperConfigPrinter? Printer { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pause_resume")]
        public partial KlipperStatusPauseResume? PauseResume { get; set; }

        [ObservableProperty]
        
        [JsonProperty("display_status")]
        public partial KlipperStatusDisplay? DisplayStatus { get; set; }

        [ObservableProperty]
        
        [JsonProperty("stepper_y")]
        public partial KlipperConfigStepper? StepperY { get; set; }

        [ObservableProperty]
        
        [JsonProperty("stepper_x")]
        public partial KlipperConfigStepper? StepperX { get; set; }

        [ObservableProperty]
        
        [JsonProperty("gcode_macro RESUME")]
        public partial KlipperGcodeMacro? GcodeMacroResume { get; set; }

        [ObservableProperty]
        
        [JsonProperty("gcode_macro PAUSE")]
        public partial KlipperGcodeMacro? GcodeMacroPause { get; set; }

        [ObservableProperty]
        
        [JsonProperty("gcode_macro CANCEL_PRINT")]
        public partial KlipperGcodeMacro? GcodeMacroCancelPrint { get; set; }

        [ObservableProperty]
        
        [JsonProperty("fan")]
        public partial KlipperConfigFan? Fan { get; set; }

        [ObservableProperty]
        
        [JsonProperty("stepper_z")]
        public partial KlipperConfigStepper? StepperZ { get; set; }

        [ObservableProperty]
        
        [JsonProperty("mcu")]
        public partial KlipperConfigMcu? Mcu { get; set; }

        [ObservableProperty]
        
        [JsonProperty("display")]
        public partial KlipperConfigDisplay? Display { get; set; }

        [ObservableProperty]
        
        [JsonProperty("extruder")]
        public partial KlipperConfigExtruder? Extruder { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
