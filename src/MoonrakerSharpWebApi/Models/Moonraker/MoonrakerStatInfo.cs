using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerStatInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("time")]
        double? time;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("cpu_usage")]
        double? cpuUsage;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("memory")]
        long? memory;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("mem_units", NullValueHandling = NullValueHandling.Ignore)]
        MoonrakerMemUnits? memUnits;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
