namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStateMessageResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("state_message")]
        public partial string StateMessage { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("klipper_path")]
        public partial string KlipperPath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("config_file")]
        public partial string ConfigFile { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("software_version")]
        public partial string SoftwareVersion { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("hostname")]
        public partial string Hostname { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("cpu_info")]
        public partial string CpuInfo { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("state")]
        public partial string State { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("python_path")]
        public partial string PythonPath { get; set; } = string.Empty;

        [ObservableProperty]

        [JsonPropertyName("log_file")]
        public partial string LogFile { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonSerializer.Serialize(this!, MoonrakerClientSourceGenerationContext.Default.);
        #endregion
    }
}
