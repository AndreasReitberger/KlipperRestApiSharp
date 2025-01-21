using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserActionResult : ObservableObject

    {
        #region Properties
        [ObservableProperty]
        
        [JsonProperty("username")]
        public partial string Username { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("token")]
        public partial string Token { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("refresh_token")]
        public partial string RefreshToken { get; set; } = string.Empty;

        [ObservableProperty]
        
        [JsonProperty("action")]
        public partial string Action { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
