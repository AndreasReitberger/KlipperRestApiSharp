using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServer
    {
        #region Properties
        [JsonProperty("host")]
        public string Host { get; set; } = string.Empty;

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("ssl_port")]
        public long SslPort { get; set; }

        [JsonProperty("klippy_uds_address")]
        public string KlippyUdsAddress { get; set; } = string.Empty;

        [JsonProperty("max_upload_size")]
        public long MaxUploadSize { get; set; }

        [JsonProperty("ssl_certificate_path")]
        public object? SslCertificatePath { get; set; }

        [JsonProperty("ssl_key_path")]
        public object? SslKeyPath { get; set; }

        [JsonProperty("enable_debug_logging")]
        public bool EnableDebugLogging { get; set; }

        [JsonProperty("enable_database_debug")]
        public bool EnableDatabaseDebug { get; set; }

        [JsonProperty("database_path")]
        public string DatabasePath { get; set; } = string.Empty;

        [JsonProperty("queue_gcode_uploads")]
        public bool QueueGcodeUploads { get; set; }

        [JsonProperty("config_path")]
        public string ConfigPath { get; set; } = string.Empty;

        [JsonProperty("log_path")]
        public string LogPath { get; set; } = string.Empty;

        [JsonProperty("temperature_store_size")]
        public long TemperatureStoreSize { get; set; }

        [JsonProperty("gcode_store_size")]
        public long GcodeStoreSize { get; set; }

        [JsonProperty("load_on_startup")]
        public bool LoadOnStartup { get; set; }

        [JsonProperty("automatic_transition")]
        public bool AutomaticTransition { get; set; }

        [JsonProperty("job_transition_delay")]
        public double JobTransitionDelay { get; set; }

        [JsonProperty("job_transition_gcode")]
        public string JobTransitionGcode { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
