using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDistribution : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("id")]
        string id = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("version")]
        long version;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("version_parts")]
        KlipperVersion? klipperVersion;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("like")]
        string like = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("codename")]
        string codename = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
