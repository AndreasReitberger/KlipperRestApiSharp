using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperFileItem : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("path")]
        string path = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("root")]
        string root = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
