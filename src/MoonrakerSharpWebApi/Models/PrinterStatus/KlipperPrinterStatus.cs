using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatus : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("virtual_sdcard")]
        KlipperStatusVirtualSdcard? virtualSdcard;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heaters")]
        KlipperStatusHeaters? heaters;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("pause_resume")]
        KlipperStatusPauseResume? pauseResume;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("display_status")]
        KlipperStatusDisplay? displayStatus;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("idle_timeout")]
        KlipperStatusIdleTimeout? idleTimeout;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("system_stats")]
        KlipperStatusSystemStats? systemStats;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("print_stats")]
        KlipperStatusPrintStats? printStats;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("query_endstops")]
        KlipperStatusQueryEndstops? queryEndstops;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("fan")]
        KlipperStatusFan? fan;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("motion_report")]
        KlipperStatusMotionReport? motionReport;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("configfile")]
        KlipperStatusConfigfile? configfile;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("menu")]
        KlipperStatusMenu? menu;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mcu")]
        KlipperStatusMcu? mcu;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("webhooks")]
        KlipperStatusWebhooks? webhooks;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_move")]
        KlipperStatusGcodeMove? gcodeMove;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("toolhead")]
        KlipperStatusToolhead? toolhead;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder")]
        KlipperStatusExtruder? extruder;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder1")]
        KlipperStatusExtruder? extruder1;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder2")]
        KlipperStatusExtruder? extruder2;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("extruder3")]
        KlipperStatusExtruder? extruder3;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("heater_bed")]
        KlipperStatusHeaterBed? heaterBed;
        #endregion 

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        
        #endregion
    }
}
