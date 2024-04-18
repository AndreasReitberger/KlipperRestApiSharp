using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobResult : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintAbiJobInfoFile? file;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("estimatedPrintTime", NullValueHandling = NullValueHandling.Ignore)]
        long estimatedPrintTime;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("filament", NullValueHandling = NullValueHandling.Ignore)]
        OctoprintApiFilamentInfo? filament;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
        object? user;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
