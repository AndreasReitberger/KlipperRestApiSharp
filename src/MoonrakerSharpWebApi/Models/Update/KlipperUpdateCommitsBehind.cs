using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateCommitsBehind : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sha")]
        string sha = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("author")]
        string author = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("date")]
        long date;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("subject")]
        string subject = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("message")]
        string message = string.Empty;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("tag")]
        object? tag;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
