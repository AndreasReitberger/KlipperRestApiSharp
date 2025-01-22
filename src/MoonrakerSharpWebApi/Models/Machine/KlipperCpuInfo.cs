using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperCpuInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("cpu_count")]
        public partial long CpuCount { get; set; }

        [ObservableProperty]
        
        [JsonProperty("bits")]
        public partial string Bits { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("processor")]
        public partial string Processor { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("cpu_desc")]
        public partial string CpuDesc { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("serial_number")]
        public partial string SerialNumber { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("hardware_desc")]
        public partial string HardwareDesc { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("model")]
        public partial string Model { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("total_memory")]
        public partial long TotalMemory { get; set; }

        [ObservableProperty]
        
        [JsonProperty("memory_units")]
        public partial string MemoryUnits { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
