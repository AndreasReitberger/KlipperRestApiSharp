using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperCpuInfo
    {
        #region Properties
        [JsonProperty("cpu_count")]
        public long CpuCount { get; set; }

        [JsonProperty("bits")]
        public string Bits { get; set; }

        [JsonProperty("processor")]
        public string Processor { get; set; }

        [JsonProperty("cpu_desc")]
        public string CpuDesc { get; set; }

        [JsonProperty("serial_number")]
        public string SerialNumber { get; set; }

        [JsonProperty("hardware_desc")]
        public string HardwareDesc { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("total_memory")]
        public long TotalMemory { get; set; }

        [JsonProperty("memory_units")]
        public string MemoryUnits { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
