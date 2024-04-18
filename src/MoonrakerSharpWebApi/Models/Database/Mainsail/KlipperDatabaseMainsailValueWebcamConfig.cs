using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        public string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("icon")]
        public string icon = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("service")]
        public string service = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("targetFps")]
        public long targetFps;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("url")]
        public string url = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipX")]
        public bool flipX;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipY")]
        public bool flipY;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
