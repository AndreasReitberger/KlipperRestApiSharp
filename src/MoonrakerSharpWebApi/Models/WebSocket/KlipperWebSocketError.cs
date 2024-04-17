using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketError : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("code")]
        long code;

        [ObservableProperty]
        [property: JsonProperty("message")]
        string message = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
