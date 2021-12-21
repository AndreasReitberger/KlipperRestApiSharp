using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperSettings
    {
        #region Properties
        [JsonProperty("virtual_sdcard")]
        public KlipperConfigVirtualSdcard VirtualSdcard { get; set; }

        [JsonProperty("printer")]
        public KlipperConfigPrinter Printer { get; set; }

        [JsonProperty("verify_heater extruder")]
        public KlipperSettingVerifyHeaterExtruder VerifyHeaterExtruder { get; set; }

        [JsonProperty("force_move")]
        public KlipperSettingForceMove ForceMove { get; set; }

        [JsonProperty("pause_resume")]
        public KlipperSettingPauseResume PauseResume { get; set; }

        [JsonProperty("stepper_z")]
        public KlipperConfigStepper StepperZ { get; set; }

        [JsonProperty("stepper_y")]
        public KlipperConfigStepper StepperY { get; set; }

        [JsonProperty("stepper_x")]
        public KlipperConfigStepper StepperX { get; set; }

        /*
        [JsonProperty("gcode_macro resume")]
        public KlipperGcodeMacro GcodeMacroResume { get; set; }

        [JsonProperty("gcode_macro pause")]
        public KlipperGcodeMacro GcodeMacroPause { get; set; }

        [JsonProperty("gcode_macro cancel_print")]
        public KlipperGcodeMacro GcodeMacroCancelPrint { get; set; }
        */

        [JsonProperty("idle_timeout")]
        public KlipperSettingIdleTimeout IdleTimeout { get; set; }

        [JsonProperty("fan")]
        public KlipperConfigFan Fan { get; set; }

        [JsonProperty("mcu")]
        public KlipperConfigMcu Mcu { get; set; }

        [JsonProperty("display")]
        public KlipperSettingsDisplay Display { get; set; }

        [JsonProperty("extruder")]
        public KlipperConfigExtruder Extruder { get; set; }

        [JsonProperty("heater_bed")]
        public KlipperConfigHeaterBed HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
