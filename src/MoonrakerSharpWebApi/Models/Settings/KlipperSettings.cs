using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("virtual_sdcard")]
        KlipperConfigVirtualSdcard? virtualSdcard;

        [ObservableProperty]
        [property: JsonProperty("printer")]
        KlipperConfigPrinter? printer;

        [ObservableProperty]
        [property: JsonProperty("verify_heater extruder")]
        KlipperSettingVerifyHeaterExtruder? verifyHeaterExtruder;

        [ObservableProperty]
        [property: JsonProperty("force_move")]
        KlipperSettingForceMove? forceMove;

        [ObservableProperty]
        [property: JsonProperty("pause_resume")]
        KlipperSettingPauseResume? pauseResume;

        [ObservableProperty]
        [property: JsonProperty("stepper_z")]
        KlipperConfigStepper? stepperZ;

        [ObservableProperty]
        [property: JsonProperty("stepper_y")]
        KlipperConfigStepper? stepperY;

        [ObservableProperty]
        [property: JsonProperty("stepper_x")]
        KlipperConfigStepper? stepperX;

        [ObservableProperty]
        [property: JsonProperty("idle_timeout")]
        KlipperSettingIdleTimeout? idleTimeout;

        [ObservableProperty]
        [property: JsonProperty("fan")]
        KlipperConfigFan? fan;

        [ObservableProperty]
        [property: JsonProperty("mcu")]
        KlipperConfigMcu? mcu;

        [ObservableProperty]
        [property: JsonProperty("display")]
        KlipperSettingsDisplay? display;

        [ObservableProperty]
        [property: JsonProperty("extruder")]
        KlipperConfigExtruder? extruder;

        [ObservableProperty]
        [property: JsonProperty("heater_bed")]
        KlipperConfigHeaterBed? heaterBed;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
