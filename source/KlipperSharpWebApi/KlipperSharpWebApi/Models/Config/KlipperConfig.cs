using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperConfig
    {
        #region Properties
        [JsonProperty("virtual_sdcard")]
        public KlipperConfigVirtualSdcard VirtualSdcard { get; set; }

        [JsonProperty("printer")]
        public KlipperConfigPrinter Printer { get; set; }

        [JsonProperty("pause_resume")]
        public KlipperStatusPauseResume PauseResume { get; set; }

        [JsonProperty("display_status")]
        public KlipperStatusDisplay DisplayStatus { get; set; }

        [JsonProperty("stepper_y")]
        public KlipperConfigStepper StepperY { get; set; }

        [JsonProperty("stepper_x")]
        public KlipperConfigStepper StepperX { get; set; }

        [JsonProperty("gcode_macro RESUME")]
        public KlipperGcodeMacro GcodeMacroResume { get; set; }

        [JsonProperty("gcode_macro PAUSE")]
        public KlipperGcodeMacro GcodeMacroPause { get; set; }

        [JsonProperty("gcode_macro CANCEL_PRINT")]
        public KlipperGcodeMacro GcodeMacroCancelPrint { get; set; }

        [JsonProperty("fan")]
        public KlipperConfigFan Fan { get; set; }

        [JsonProperty("stepper_z")]
        public KlipperConfigStepper StepperZ { get; set; }

        [JsonProperty("mcu")]
        public KlipperConfigMcu Mcu { get; set; }

        [JsonProperty("display")]
        public KlipperConfigDisplay Display { get; set; }
        // Actually just a list of pin names
        //public Dictionary<string, string> Display { get; set; } = new();

        [JsonProperty("extruder")]
        public KlipperConfigExtruder Extruder { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
