using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("id")]
        Guid id;

        [ObservableProperty]
        [property: JsonProperty("enabled")]
        bool enabled;

        [ObservableProperty]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("service")]
        string service = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("targetFps")]
        long fpstarget;

        [ObservableProperty]
        [property: JsonProperty("urlStream")]
        string urlStream = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("urlSnapshot")]
        string urlSnapshot = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("flipX")]
        bool flipX;

        [ObservableProperty]
        [property: JsonProperty("flipY")]
        bool flipY;

        [ObservableProperty]
        [property: JsonProperty("rotation")]
        int? rotation = 0;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
