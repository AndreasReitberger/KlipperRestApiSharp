using AndreasReitberger.API.Moonraker.Enum;
using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class MoonrakerStatInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("time")]
        double? time;

        [ObservableProperty]
        [property: JsonProperty("cpu_usage")]
        double? cpuUsage;

        [ObservableProperty]
        [property: JsonProperty("memory")]
        long? memory;

        [ObservableProperty]
        [property: JsonProperty("mem_units", NullValueHandling = NullValueHandling.Ignore)]
        MoonrakerMemUnits? memUnits;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
