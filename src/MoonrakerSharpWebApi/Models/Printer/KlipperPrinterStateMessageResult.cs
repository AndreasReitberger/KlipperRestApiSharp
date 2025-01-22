using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStateMessageResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("state_message")]
        public partial string StateMessage { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("klipper_path")]
        public partial string KlipperPath { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("config_file")]
        public partial string ConfigFile { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("software_version")]
        public partial string SoftwareVersion { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("hostname")]
        public partial string Hostname { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("cpu_info")]
        public partial string CpuInfo { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("state")]
        public partial string State { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("python_path")]
        public partial string PythonPath { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("log_file")]
        public partial string LogFile { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
