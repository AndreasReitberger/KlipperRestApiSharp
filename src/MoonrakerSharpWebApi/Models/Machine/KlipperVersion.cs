using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVersion : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("major")]
        long major;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("minor")]
        string minor = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("build_number")]
        string buildNumber = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
