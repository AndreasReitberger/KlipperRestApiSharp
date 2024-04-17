using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUserActionResult : ObservableObject

    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("username")]
        string username = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("token")]
        string token = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("refresh_token")]
        string refreshToken = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("action")]
        string action = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
