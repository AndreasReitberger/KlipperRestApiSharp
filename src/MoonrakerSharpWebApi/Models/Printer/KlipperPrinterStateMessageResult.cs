using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStateMessageResult
    {
        #region Properties
        [JsonProperty("state_message")]
        public string StateMessage { get; set; } = string.Empty;

        [JsonProperty("klipper_path")]
        public string KlipperPath { get; set; } = string.Empty;

        [JsonProperty("config_file")]
        public string ConfigFile { get; set; } = string.Empty;

        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; } = string.Empty;

        [JsonProperty("hostname")]
        public string Hostname { get; set; } = string.Empty;

        [JsonProperty("cpu_info")]
        public string CpuInfo { get; set; } = string.Empty;

        [JsonProperty("state")]
        public string State { get; set; } = string.Empty;

        [JsonProperty("python_path")]
        public string PythonPath { get; set; } = string.Empty;

        [JsonProperty("log_file")]
        public string LogFile { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
