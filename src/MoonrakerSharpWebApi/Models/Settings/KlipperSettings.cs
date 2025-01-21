using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("virtual_sdcard")]
        public partial KlipperConfigVirtualSdcard? VirtualSdcard { get; set; }

        [ObservableProperty]
        
        [JsonProperty("printer")]
        public partial KlipperConfigPrinter? Printer { get; set; }

        [ObservableProperty]
        
        [JsonProperty("verify_heater extruder")]
        public partial KlipperSettingVerifyHeaterExtruder? VerifyHeaterExtruder { get; set; }

        [ObservableProperty]
        
        [JsonProperty("force_move")]
        public partial KlipperSettingForceMove? ForceMove { get; set; }

        [ObservableProperty]
        
        [JsonProperty("pause_resume")]
        public partial KlipperSettingPauseResume? PauseResume { get; set; }

        [ObservableProperty]
        
        [JsonProperty("stepper_z")]
        public partial KlipperConfigStepper? StepperZ { get; set; }

        [ObservableProperty]
        
        [JsonProperty("stepper_y")]
        public partial KlipperConfigStepper? StepperY { get; set; }

        [ObservableProperty]
        
        [JsonProperty("stepper_x")]
        public partial KlipperConfigStepper? StepperX { get; set; }

        [ObservableProperty]
        
        [JsonProperty("idle_timeout")]
        public partial KlipperSettingIdleTimeout? IdleTimeout { get; set; }

        [ObservableProperty]
        
        [JsonProperty("fan")]
        public partial KlipperConfigFan? Fan { get; set; }

        [ObservableProperty]
        
        [JsonProperty("mcu")]
        public partial KlipperConfigMcu? Mcu { get; set; }

        [ObservableProperty]
        
        [JsonProperty("display")]
        public partial KlipperSettingsDisplay? Display { get; set; }

        [ObservableProperty]
        
        [JsonProperty("extruder")]
        public partial KlipperConfigExtruder? Extruder { get; set; }

        [ObservableProperty]
        
        [JsonProperty("heater_bed")]
        public partial KlipperConfigHeaterBed? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
