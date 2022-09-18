using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerStatInfo
    {
        #region Properties
        [JsonProperty("time")]
        public double? Time { get; set; }

        [JsonProperty("cpu_usage")]
        public double? CpuUsage { get; set; }

        [JsonProperty("memory")]
        public long? Memory { get; set; }

        [JsonProperty("mem_units")]
        public MoonrakerMemUnits MemUnits { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
