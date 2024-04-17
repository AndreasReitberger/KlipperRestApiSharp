using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("virtual_sdcard")]
        KlipperConfigVirtualSdcard? virtualSdcard;

        [ObservableProperty]
        [property: JsonProperty("printer")]
        KlipperConfigPrinter? printer;

        [ObservableProperty]
        [property: JsonProperty("pause_resume")]
        KlipperStatusPauseResume? pauseResume;

        [ObservableProperty]
        [property: JsonProperty("display_status")]
        KlipperStatusDisplay? displayStatus;

        [ObservableProperty]
        [property: JsonProperty("stepper_y")]
        KlipperConfigStepper? stepperY;

        [ObservableProperty]
        [property: JsonProperty("stepper_x")]
        KlipperConfigStepper? stepperX;

        [ObservableProperty]
        [property: JsonProperty("gcode_macro RESUME")]
        KlipperGcodeMacro? gcodeMacroResume;

        [ObservableProperty]
        [property: JsonProperty("gcode_macro PAUSE")]
        KlipperGcodeMacro? gcodeMacroPause;

        [ObservableProperty]
        [property: JsonProperty("gcode_macro CANCEL_PRINT")]
        KlipperGcodeMacro? gcodeMacroCancelPrint;

        [ObservableProperty]
        [property: JsonProperty("fan")]
        KlipperConfigFan? fan;

        [ObservableProperty]
        [property: JsonProperty("stepper_z")]
        KlipperConfigStepper? stepperZ;

        [ObservableProperty]
        [property: JsonProperty("mcu")]
        KlipperConfigMcu? mcu;

        [ObservableProperty]
        [property: JsonProperty("display")]
        KlipperConfigDisplay? display;

        [ObservableProperty]
        [property: JsonProperty("extruder")]
        KlipperConfigExtruder? extruder;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
