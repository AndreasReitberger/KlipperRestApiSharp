using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServer : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("host")]
        public partial string Host { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("port")]
        public partial long Port { get; set; }

        [ObservableProperty]

        [JsonProperty("ssl_port")]
        public partial long SslPort { get; set; }

        [ObservableProperty]

        [JsonProperty("klippy_uds_address")]
        public partial string KlippyUdsAddress { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("max_upload_size")]
        public partial long MaxUploadSize { get; set; }

        [ObservableProperty]

        [JsonProperty("ssl_certificate_path")]
        public partial object? SslCertificatePath { get; set; }

        [ObservableProperty]

        [JsonProperty("ssl_key_path")]
        public partial object? SslKeyPath { get; set; }

        [ObservableProperty]

        [JsonProperty("enable_debug_logging")]
        public partial bool EnableDebugLogging { get; set; }

        [ObservableProperty]

        [JsonProperty("enable_database_debug")]
        public partial bool EnableDatabaseDebug { get; set; }

        [ObservableProperty]

        [JsonProperty("database_path")]
        public partial string DatabasePath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("queue_gcode_uploads")]
        public partial bool QueueGcodeUploads { get; set; }

        [ObservableProperty]

        [JsonProperty("config_path")]
        public partial string ConfigPath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("log_path")]
        public partial string LogPath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonProperty("temperature_store_size")]
        public partial long TemperatureStoreSize { get; set; }

        [ObservableProperty]

        [JsonProperty("gcode_store_size")]
        public partial long GcodeStoreSize { get; set; }

        [ObservableProperty]

        [JsonProperty("load_on_startup")]
        public partial bool LoadOnStartup { get; set; }

        [ObservableProperty]

        [JsonProperty("automatic_transition")]
        public partial bool AutomaticTransition { get; set; }

        [ObservableProperty]

        [JsonProperty("job_transition_delay")]
        public partial double JobTransitionDelay { get; set; }

        [ObservableProperty]

        [JsonProperty("job_transition_gcode")]
        public partial string JobTransitionGcode { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
