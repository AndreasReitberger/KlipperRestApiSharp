using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperConfigVirtualSdcard : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("path")]
        public partial string Path { get; set; } = string.Empty;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
