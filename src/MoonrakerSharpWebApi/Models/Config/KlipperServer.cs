namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServer : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [JsonPropertyName("host")]
        public partial string Host { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("port")]
        public partial long Port { get; set; }

        [ObservableProperty]
        [JsonPropertyName("ssl_port")]
        public partial long SslPort { get; set; }

        [ObservableProperty]
        [JsonPropertyName("klippy_uds_address")]
        public partial string KlippyUdsAddress { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("max_upload_size")]
        public partial long MaxUploadSize { get; set; }

        [ObservableProperty]
        [JsonPropertyName("ssl_certificate_path")]
        public partial object? SslCertificatePath { get; set; }

        [ObservableProperty]
        [JsonPropertyName("ssl_key_path")]
        public partial object? SslKeyPath { get; set; }

        [ObservableProperty]
        [JsonPropertyName("enable_debug_logging")]
        public partial bool EnableDebugLogging { get; set; }

        [ObservableProperty]
        [JsonPropertyName("enable_database_debug")]
        public partial bool EnableDatabaseDebug { get; set; }

        [ObservableProperty]
        [JsonPropertyName("database_path")]
        public partial string DatabasePath { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("queue_gcode_uploads")]
        public partial bool QueueGcodeUploads { get; set; }

        [ObservableProperty]
        [JsonPropertyName("config_path")]
        public partial string ConfigPath { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("log_path")]
        public partial string LogPath { get; set; } = string.Empty;

        [ObservableProperty]
        [JsonPropertyName("temperature_store_size")]
        public partial long TemperatureStoreSize { get; set; }

        [ObservableProperty]
        [JsonPropertyName("gcode_store_size")]
        public partial long GcodeStoreSize { get; set; }

        [ObservableProperty]
        [JsonPropertyName("load_on_startup")]
        public partial bool LoadOnStartup { get; set; }

        [ObservableProperty]
        [JsonPropertyName("automatic_transition")]
        public partial bool AutomaticTransition { get; set; }

        [ObservableProperty]
        [JsonPropertyName("job_transition_delay")]
        public partial double JobTransitionDelay { get; set; }

        [ObservableProperty]
        [JsonPropertyName("job_transition_gcode")]
        public partial string JobTransitionGcode { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.KlipperServer);
        #endregion
    }

}
