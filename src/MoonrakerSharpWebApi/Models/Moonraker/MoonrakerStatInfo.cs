using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerStatInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("time")]
        public partial double? Time { get; set; }

        [ObservableProperty]
        
        [JsonProperty("cpu_usage")]
        public partial double? CpuUsage { get; set; }

        [ObservableProperty]
        
        [JsonProperty("memory")]
        public partial long? Memory { get; set; }

        [ObservableProperty]
        
        [JsonProperty("mem_units", NullValueHandling = NullValueHandling.Ignore)]
        public partial MoonrakerMemUnits? MemUnits { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
