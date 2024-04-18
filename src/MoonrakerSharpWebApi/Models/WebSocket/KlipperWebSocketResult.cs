using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("jsonrpc")]
        string jsonrpc = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("result")]
        object? result = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("id")]
        long id;
    
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
