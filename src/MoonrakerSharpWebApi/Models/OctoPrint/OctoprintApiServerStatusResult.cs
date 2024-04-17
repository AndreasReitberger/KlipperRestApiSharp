using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiServerStatusResult : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("server")]
        string server = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("safemode")]
        object? safemode;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
