using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperPrinterStateMessageResult
    {
        #region Properties
        [JsonProperty("state_message")]
        public string StateMessage { get; set; }

        [JsonProperty("klipper_path")]
        public string KlipperPath { get; set; }

        [JsonProperty("config_file")]
        public string ConfigFile { get; set; }

        [JsonProperty("software_version")]
        public string SoftwareVersion { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("cpu_info")]
        public string CpuInfo { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("python_path")]
        public string PythonPath { get; set; }

        [JsonProperty("log_file")]
        public string LogFile { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
