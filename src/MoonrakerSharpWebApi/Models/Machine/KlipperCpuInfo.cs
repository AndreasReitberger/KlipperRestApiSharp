using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperCpuInfo
    {
        #region Properties
        [JsonProperty("cpu_count")]
        public long CpuCount { get; set; }

        [JsonProperty("bits")]
        public string Bits { get; set; } = string.Empty;

        [JsonProperty("processor")]
        public string Processor { get; set; } = string.Empty;

        [JsonProperty("cpu_desc")]
        public string CpuDesc { get; set; } = string.Empty;

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonProperty("hardware_desc")]
        public string HardwareDesc { get; set; } = string.Empty;

        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        [JsonProperty("total_memory")]
        public long TotalMemory { get; set; }

        [JsonProperty("memory_units")]
        public string MemoryUnits { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
