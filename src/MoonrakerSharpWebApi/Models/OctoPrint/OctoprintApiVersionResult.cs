using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiVersionResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("server")]
        public partial string Server { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("api")]
        public partial string Api { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("text")]
        public partial string Text { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
