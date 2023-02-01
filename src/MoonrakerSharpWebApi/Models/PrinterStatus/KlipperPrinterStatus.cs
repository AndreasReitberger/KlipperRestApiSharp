using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatus
    {
        #region Properties
        [JsonProperty("virtual_sdcard")]
        public KlipperStatusVirtualSdcard VirtualSdcard { get; set; }

        [JsonProperty("heaters")]
        public KlipperStatusHeaters Heaters { get; set; }

        [JsonProperty("pause_resume")]
        public KlipperStatusPauseResume PauseResume { get; set; }

        [JsonProperty("display_status")]
        public KlipperStatusDisplay DisplayStatus { get; set; }

        [JsonProperty("idle_timeout")]
        public KlipperStatusIdleTimeout IdleTimeout { get; set; }

        [JsonProperty("system_stats")]
        public KlipperStatusSystemStats SystemStats { get; set; }

        [JsonProperty("print_stats")]
        public KlipperStatusPrintStats PrintStats { get; set; }

        [JsonProperty("query_endstops")]
        public KlipperStatusQueryEndstops QueryEndstops { get; set; }

        [JsonProperty("fan")]
        public KlipperStatusFan Fan { get; set; }

        [JsonProperty("motion_report")]
        public KlipperStatusMotionReport MotionReport { get; set; }

        [JsonProperty("configfile")]
        public KlipperStatusConfigfile Configfile { get; set; }

        [JsonProperty("menu")]
        public KlipperStatusMenu Menu { get; set; }

        [JsonProperty("mcu")]
        public KlipperStatusMcu Mcu { get; set; }

        [JsonProperty("webhooks")]
        public KlipperStatusWebhooks Webhooks { get; set; }

        [JsonProperty("gcode_move")]
        public KlipperStatusGcodeMove GcodeMove { get; set; }

        [JsonProperty("toolhead")]
        public KlipperStatusToolhead Toolhead { get; set; }

        [JsonProperty("extruder")]
        public KlipperStatusExtruder Extruder { get; set; }

        [JsonProperty("extruder1")]
        public KlipperStatusExtruder Extruder1 { get; set; }

        [JsonProperty("extruder2")]
        public KlipperStatusExtruder Extruder2 { get; set; }

        [JsonProperty("extruder3")]
        public KlipperStatusExtruder Extruder3 { get; set; }

        [JsonProperty("heater_bed")]
        public KlipperStatusHeaterBed HeaterBed { get; set; }
        #endregion 

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
