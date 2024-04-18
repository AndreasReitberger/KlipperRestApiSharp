using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("virtual_sdcard")]
        KlipperConfigVirtualSdcard? virtualSdcard;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("printer")]
        KlipperConfigPrinter? printer;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pause_resume")]
        KlipperStatusPauseResume? pauseResume;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display_status")]
        KlipperStatusDisplay? displayStatus;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("stepper_y")]
        KlipperConfigStepper? stepperY;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("stepper_x")]
        KlipperConfigStepper? stepperX;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_macro RESUME")]
        KlipperGcodeMacro? gcodeMacroResume;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_macro PAUSE")]
        KlipperGcodeMacro? gcodeMacroPause;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_macro CANCEL_PRINT")]
        KlipperGcodeMacro? gcodeMacroCancelPrint;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("fan")]
        KlipperConfigFan? fan;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("stepper_z")]
        KlipperConfigStepper? stepperZ;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mcu")]
        KlipperConfigMcu? mcu;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display")]
        KlipperConfigDisplay? display;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder")]
        KlipperConfigExtruder? extruder;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
