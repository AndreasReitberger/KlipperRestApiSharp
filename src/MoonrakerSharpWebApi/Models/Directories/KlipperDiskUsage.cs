using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDiskUsage : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("total")]
        long total;

        [ObservableProperty]
        [property: JsonProperty("used")]
        long used;

        [ObservableProperty]
        [property: JsonProperty("free")]
        long free;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
