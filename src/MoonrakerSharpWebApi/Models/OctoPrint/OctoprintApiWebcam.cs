using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiWebcam : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("flipH")]
        bool flipH;

        [ObservableProperty]
        [property: JsonProperty("flipV")]
        bool flipV;

        [ObservableProperty]
        [property: JsonProperty("rotate90")]
        bool rotate90;

        [ObservableProperty]
        [property: JsonProperty("streamUrl")]
        string streamUrl = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("webcamEnabled")]
        bool webcamEnabled;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
