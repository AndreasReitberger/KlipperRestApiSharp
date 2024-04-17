using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseMainsailValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("name")]
        public string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("icon")]
        public string icon = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("service")]
        public string service = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("targetFps")]
        public long targetFps;

        [ObservableProperty]
        [property: JsonProperty("url")]
        public string url = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("flipX")]
        public bool flipX;

        [ObservableProperty]
        [property: JsonProperty("flipY")]
        public bool flipY;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
