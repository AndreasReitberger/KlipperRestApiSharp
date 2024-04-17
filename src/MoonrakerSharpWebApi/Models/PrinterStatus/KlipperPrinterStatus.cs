using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatus : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("virtual_sdcard")]
        KlipperStatusVirtualSdcard? virtualSdcard;

        [ObservableProperty]
        [property: JsonProperty("heaters")]
        KlipperStatusHeaters? heaters;

        [ObservableProperty]
        [property: JsonProperty("pause_resume")]
        KlipperStatusPauseResume? pauseResume;

        [ObservableProperty]
        [property: JsonProperty("display_status")]
        KlipperStatusDisplay? displayStatus;

        [ObservableProperty]
        [property: JsonProperty("idle_timeout")]
        KlipperStatusIdleTimeout? idleTimeout;

        [ObservableProperty]
        [property: JsonProperty("system_stats")]
        KlipperStatusSystemStats? systemStats;

        [ObservableProperty]
        [property: JsonProperty("print_stats")]
        KlipperStatusPrintStats? printStats;

        [ObservableProperty]
        [property: JsonProperty("query_endstops")]
        KlipperStatusQueryEndstops? queryEndstops;

        [ObservableProperty]
        [property: JsonProperty("fan")]
        KlipperStatusFan? fan;

        [ObservableProperty]
        [property: JsonProperty("motion_report")]
        KlipperStatusMotionReport? motionReport;

        [ObservableProperty]
        [property: JsonProperty("configfile")]
        KlipperStatusConfigfile? configfile;

        [ObservableProperty]
        [property: JsonProperty("menu")]
        KlipperStatusMenu? menu;

        [ObservableProperty]
        [property: JsonProperty("mcu")]
        KlipperStatusMcu? mcu;

        [ObservableProperty]
        [property: JsonProperty("webhooks")]
        KlipperStatusWebhooks? webhooks;

        [ObservableProperty]
        [property: JsonProperty("gcode_move")]
        KlipperStatusGcodeMove? gcodeMove;

        [ObservableProperty]
        [property: JsonProperty("toolhead")]
        KlipperStatusToolhead? toolhead;

        [ObservableProperty]
        [property: JsonProperty("extruder")]
        KlipperStatusExtruder? extruder;

        [ObservableProperty]
        [property: JsonProperty("extruder1")]
        KlipperStatusExtruder? extruder1;

        [ObservableProperty]
        [property: JsonProperty("extruder2")]
        KlipperStatusExtruder? extruder2;

        [ObservableProperty]
        [property: JsonProperty("extruder3")]
        KlipperStatusExtruder? extruder3;

        [ObservableProperty]
        [property: JsonProperty("heater_bed")]
        KlipperStatusHeaterBed? heaterBed;
        #endregion 

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        
        #endregion
    }
}
