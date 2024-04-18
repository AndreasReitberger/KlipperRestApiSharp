using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateManagerMainsail : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("type")]
        string type = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("repo")]
        string repo = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("path")]
        string path = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("persistent_files")]
        object? persistentFiles;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
