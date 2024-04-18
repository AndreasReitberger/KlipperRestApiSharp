using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipH")]
        bool flipH;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipV")]
        bool flipV;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rotate90")]
        bool rotate90;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("streamUrl")]
        string streamUrl = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("webcamEnabled")]
        bool webcamEnabled;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
