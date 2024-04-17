using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterVolume : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("depth")]
        long depth;

        [ObservableProperty]
        [property: JsonProperty("formFactor")]
        string formFactor = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("height")]
        long height;

        [ObservableProperty]
        [property: JsonProperty("origin")]
        string origin = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("width")]
        long width;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
