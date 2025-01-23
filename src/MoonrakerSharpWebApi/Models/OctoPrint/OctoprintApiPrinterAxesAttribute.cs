using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxesAttribute : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("inverted")]
        public partial bool Inverted { get; set; }

        [ObservableProperty]

        [JsonProperty("speed")]
        public partial long Speed { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
