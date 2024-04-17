using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperUpdateCommitsBehind : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("sha")]
        string sha = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("author")]
        string author = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("date")]
        long date;

        [ObservableProperty]
        [property: JsonProperty("subject")]
        string subject = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("message")]
        string message = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("tag")]
        object? tag;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
