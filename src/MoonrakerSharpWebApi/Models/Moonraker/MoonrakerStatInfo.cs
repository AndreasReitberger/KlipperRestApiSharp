using AndreasReitberger.API.Moonraker.Enum;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerStatInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonPropertyName("time")]
        public partial double? Time { get; set; }

        [ObservableProperty]

        [JsonPropertyName("cpu_usage")]
        public partial double? CpuUsage { get; set; }

        [ObservableProperty]

        [JsonPropertyName("memory")]
        public partial long? Memory { get; set; }

        [ObservableProperty]

        [JsonPropertyName("mem_units")]
        public partial MoonrakerMemUnits? MemUnits { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
