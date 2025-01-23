using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatus : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("virtual_sdcard")]
        public partial KlipperStatusVirtualSdcard? VirtualSdcard { get; set; }

        [ObservableProperty]

        [JsonProperty("heaters")]
        public partial KlipperStatusHeaters? Heaters { get; set; }

        [ObservableProperty]

        [JsonProperty("pause_resume")]
        public partial KlipperStatusPauseResume? PauseResume { get; set; }

        [ObservableProperty]

        [JsonProperty("display_status")]
        public partial KlipperStatusDisplay? DisplayStatus { get; set; }

        [ObservableProperty]

        [JsonProperty("idle_timeout")]
        public partial KlipperStatusIdleTimeout? IdleTimeout { get; set; }

        [ObservableProperty]

        [JsonProperty("system_stats")]
        public partial KlipperStatusSystemStats? SystemStats { get; set; }

        [ObservableProperty]

        [JsonProperty("print_stats")]
        public partial KlipperStatusPrintStats? PrintStats { get; set; }

        [ObservableProperty]

        [JsonProperty("query_endstops")]
        public partial KlipperStatusQueryEndstops? QueryEndstops { get; set; }

        [ObservableProperty]

        [JsonProperty("fan")]
        public partial KlipperStatusFan? Fan { get; set; }

        [ObservableProperty]

        [JsonProperty("motion_report")]
        public partial KlipperStatusMotionReport? MotionReport { get; set; }

        [ObservableProperty]

        [JsonProperty("configfile")]
        public partial KlipperStatusConfigfile? Configfile { get; set; }

        [ObservableProperty]

        [JsonProperty("menu")]
        public partial KlipperStatusMenu? Menu { get; set; }

        [ObservableProperty]

        [JsonProperty("mcu")]
        public partial KlipperStatusMcu? Mcu { get; set; }

        [ObservableProperty]

        [JsonProperty("webhooks")]
        public partial KlipperStatusWebhooks? Webhooks { get; set; }

        [ObservableProperty]

        [JsonProperty("gcode_move")]
        public partial KlipperStatusGcodeMove? GcodeMove { get; set; }

        [ObservableProperty]

        [JsonProperty("toolhead")]
        public partial KlipperStatusToolhead? Toolhead { get; set; }

        [ObservableProperty]

        [JsonProperty("extruder")]
        public partial KlipperStatusExtruder? Extruder { get; set; }

        [ObservableProperty]

        [JsonProperty("extruder1")]
        public partial KlipperStatusExtruder? Extruder1 { get; set; }

        [ObservableProperty]

        [JsonProperty("extruder2")]
        public partial KlipperStatusExtruder? Extruder2 { get; set; }

        [ObservableProperty]

        [JsonProperty("extruder3")]
        public partial KlipperStatusExtruder? Extruder3 { get; set; }

        [ObservableProperty]

        [JsonProperty("heater_bed")]
        public partial KlipperStatusHeaterBed? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);

        #endregion
    }
}
