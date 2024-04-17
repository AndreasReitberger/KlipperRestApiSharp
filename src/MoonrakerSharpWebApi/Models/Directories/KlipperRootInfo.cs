using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperRootInfo : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("name")]
        string name = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("permissions")]
        string permissions = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
