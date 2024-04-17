using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperIpAddress : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("family")]
        string family = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("address")]
        string address = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("is_link_local")]
        bool isLinkLocal;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
