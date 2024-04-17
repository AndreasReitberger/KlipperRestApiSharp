using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperVirtualization : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("virt_type")]
        string virtType = string.Empty;

        [ObservableProperty]
        [property: JsonProperty("virt_identifier")]
        string virtIdentifier = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
