using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperGcode : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("message")]
        public partial string Message { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("time")]
        public partial double Time { get; set; }

        [ObservableProperty]
        
        [JsonProperty("type")]
        public partial string Type { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
