using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStateMessageResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state_message")]
        string stateMessage = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("klipper_path")]
        string klipperPath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("config_file")]
        string configFile = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("software_version")]
        string softwareVersion = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hostname")]
        string hostname = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cpu_info")]
        string cpuInfo = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("state")]
        string state = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("python_path")]
        string pythonPath = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("log_file")]
        string logFile = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
