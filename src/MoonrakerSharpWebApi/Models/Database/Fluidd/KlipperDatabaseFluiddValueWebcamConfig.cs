using Newtonsoft.Json;
using System;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDatabaseFluiddValueWebcamConfig : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("id")]
        Guid id;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("enabled")]
        bool enabled;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("service")]
        string service = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("targetFps")]
        long fpstarget;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("urlStream")]
        string urlStream = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("urlSnapshot")]
        string urlSnapshot = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipX")]
        bool flipX;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("flipY")]
        bool flipY;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("rotation")]
        int? rotation = 0;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
