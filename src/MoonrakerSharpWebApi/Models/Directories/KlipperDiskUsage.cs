using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDiskUsage : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("total")]
        public partial long Total { get; set; }

        [ObservableProperty]
        
        [JsonProperty("used")]
        public partial long Used { get; set; }

        [ObservableProperty]
        
        [JsonProperty("free")]
        public partial long Free { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
