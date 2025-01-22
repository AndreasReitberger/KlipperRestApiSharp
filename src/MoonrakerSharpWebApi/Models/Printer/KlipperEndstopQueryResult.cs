using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperEndstopQueryResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("y")]
        public partial string Y { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("x")]
        public partial string X { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("z")]
        public partial string Z { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
