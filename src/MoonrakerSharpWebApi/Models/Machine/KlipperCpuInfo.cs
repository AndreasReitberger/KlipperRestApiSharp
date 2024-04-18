using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperCpuInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cpu_count")]
        long cpuCount;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("bits")]
        string bits = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("processor")]
        string processor = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cpu_desc")]
        string cpuDesc = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("serial_number")]
        string serialNumber = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("hardware_desc")]
        string hardwareDesc = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("model")]
        string model = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("total_memory")]
        long totalMemory;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("memory_units")]
        string memoryUnits = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
