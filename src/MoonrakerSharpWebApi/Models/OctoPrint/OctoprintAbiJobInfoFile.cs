using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintAbiJobInfoFile : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("origin")]
        string origin = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        long size;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("date", NullValueHandling = NullValueHandling.Ignore)]
        long date;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("path", NullValueHandling = NullValueHandling.Ignore)]
        string path = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
