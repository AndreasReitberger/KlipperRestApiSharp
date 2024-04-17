using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperDistribution : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("id")]
        string id = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("version")]
        long version;

        [ObservableProperty]
        [property: JsonProperty("version_parts")]
        KlipperVersion? klipperVersion;

        [ObservableProperty]
        [property: JsonProperty("like")]
        string like = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("codename")]
        string codename = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
