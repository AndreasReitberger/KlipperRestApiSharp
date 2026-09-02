namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatus : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("virtual_sdcard")]
        public partial KlipperStatusVirtualSdcard? VirtualSdcard { get; set; }

        [ObservableProperty]

        [JsonPropertyName("heaters")]
        public partial KlipperStatusHeaters? Heaters { get; set; }

        [ObservableProperty]

        [JsonPropertyName("pause_resume")]
        public partial KlipperStatusPauseResume? PauseResume { get; set; }

        [ObservableProperty]

        [JsonPropertyName("display_status")]
        public partial KlipperStatusDisplay? DisplayStatus { get; set; }

        [ObservableProperty]

        [JsonPropertyName("idle_timeout")]
        public partial KlipperStatusIdleTimeout? IdleTimeout { get; set; }

        [ObservableProperty]

        [JsonPropertyName("system_stats")]
        public partial KlipperStatusSystemStats? SystemStats { get; set; }

        [ObservableProperty]

        [JsonPropertyName("print_stats")]
        public partial KlipperStatusPrintStats? PrintStats { get; set; }

        [ObservableProperty]

        [JsonPropertyName("query_endstops")]
        public partial KlipperStatusQueryEndstops? QueryEndstops { get; set; }

        [ObservableProperty]

        [JsonPropertyName("fan")]
        public partial KlipperStatusFan? Fan { get; set; }

        [ObservableProperty]

        [JsonPropertyName("motion_report")]
        public partial KlipperStatusMotionReport? MotionReport { get; set; }

        [ObservableProperty]

        [JsonPropertyName("configfile")]
        public partial KlipperStatusConfigfile? Configfile { get; set; }

        [ObservableProperty]

        [JsonPropertyName("menu")]
        public partial KlipperStatusMenu? Menu { get; set; }

        [ObservableProperty]

        [JsonPropertyName("mcu")]
        public partial KlipperStatusMcu? Mcu { get; set; }

        [ObservableProperty]

        [JsonPropertyName("webhooks")]
        public partial KlipperStatusWebhooks? Webhooks { get; set; }

        [ObservableProperty]

        [JsonPropertyName("gcode_move")]
        public partial KlipperStatusGcodeMove? GcodeMove { get; set; }

        [ObservableProperty]

        [JsonPropertyName("toolhead")]
        public partial KlipperStatusToolhead? Toolhead { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extruder")]
        public partial KlipperStatusExtruder? Extruder { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extruder1")]
        public partial KlipperStatusExtruder? Extruder1 { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extruder2")]
        public partial KlipperStatusExtruder? Extruder2 { get; set; }

        [ObservableProperty]

        [JsonPropertyName("extruder3")]
        public partial KlipperStatusExtruder? Extruder3 { get; set; }

        [ObservableProperty]

        [JsonPropertyName("heater_bed")]
        public partial KlipperStatusHeaterBed? HeaterBed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);

        #endregion
    }
}
