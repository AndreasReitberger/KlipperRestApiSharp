using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServer : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("host")]
        string host = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("port")]
        long port;

        [ObservableProperty]
        [property: JsonProperty("ssl_port")]
        long sslPort;

        [ObservableProperty]
        [property: JsonProperty("klippy_uds_address")]
        string klippyUdsAddress = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("max_upload_size")]
        long maxUploadSize;

        [ObservableProperty]
        [property: JsonProperty("ssl_certificate_path")]
        object? sslCertificatePath;

        [ObservableProperty]
        [property: JsonProperty("ssl_key_path")]
        object? sslKeyPath;

        [ObservableProperty]
        [property: JsonProperty("enable_debug_logging")]
        bool enableDebugLogging;

        [ObservableProperty]
        [property: JsonProperty("enable_database_debug")]
        bool enableDatabaseDebug;

        [ObservableProperty]
        [property: JsonProperty("database_path")]
        string databasePath = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("queue_gcode_uploads")]
        bool queueGcodeUploads;

        [ObservableProperty]
        [property: JsonProperty("config_path")]
        string configPath = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("log_path")]
        string logPath = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("temperature_store_size")]
        long temperatureStoreSize;

        [ObservableProperty]
        [property: JsonProperty("gcode_store_size")]
        long gcodeStoreSize;

        [ObservableProperty]
        [property: JsonProperty("load_on_startup")]
        bool loadOnStartup;

        [ObservableProperty]
        [property: JsonProperty("automatic_transition")]
        bool automaticTransition;

        [ObservableProperty]
        [property: JsonProperty("job_transition_delay")]
        double jobTransitionDelay;

        [ObservableProperty]
        [property: JsonProperty("job_transition_gcode")]
        string jobTransitionGcode = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
