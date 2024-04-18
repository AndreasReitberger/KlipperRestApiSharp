using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperServer : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("host")]
        string host = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("port")]
        long port;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ssl_port")]
        long sslPort;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("klippy_uds_address")]
        string klippyUdsAddress = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("max_upload_size")]
        long maxUploadSize;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ssl_certificate_path")]
        object? sslCertificatePath;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("ssl_key_path")]
        object? sslKeyPath;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_debug_logging")]
        bool enableDebugLogging;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enable_database_debug")]
        bool enableDatabaseDebug;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("database_path")]
        string databasePath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("queue_gcode_uploads")]
        bool queueGcodeUploads;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("config_path")]
        string configPath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("log_path")]
        string logPath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("temperature_store_size")]
        long temperatureStoreSize;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("gcode_store_size")]
        long gcodeStoreSize;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("load_on_startup")]
        bool loadOnStartup;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("automatic_transition")]
        bool automaticTransition;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job_transition_delay")]
        double jobTransitionDelay;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("job_transition_gcode")]
        string jobTransitionGcode = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }

}
