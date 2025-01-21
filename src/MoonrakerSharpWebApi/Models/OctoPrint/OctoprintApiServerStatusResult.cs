using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiServerStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("server")]
        public partial string Server { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("safemode")]
        public partial object? Safemode { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
