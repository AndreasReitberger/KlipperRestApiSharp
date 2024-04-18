using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperSettings : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("virtual_sdcard")]
        KlipperConfigVirtualSdcard? virtualSdcard;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printer")]
        KlipperConfigPrinter? printer;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("verify_heater extruder")]
        KlipperSettingVerifyHeaterExtruder? verifyHeaterExtruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("force_move")]
        KlipperSettingForceMove? forceMove;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pause_resume")]
        KlipperSettingPauseResume? pauseResume;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("stepper_z")]
        KlipperConfigStepper? stepperZ;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("stepper_y")]
        KlipperConfigStepper? stepperY;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("stepper_x")]
        KlipperConfigStepper? stepperX;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("idle_timeout")]
        KlipperSettingIdleTimeout? idleTimeout;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("fan")]
        KlipperConfigFan? fan;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mcu")]
        KlipperConfigMcu? mcu;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display")]
        KlipperSettingsDisplay? display;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder")]
        KlipperConfigExtruder? extruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heater_bed")]
        KlipperConfigHeaterBed? heaterBed;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
