using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxes : ObservableObject
    {
        #region Properties
        [ObservableProperty]

        [JsonProperty("e")]
        public partial OctoprintApiPrinterAxesAttribute? E { get; set; }

        [ObservableProperty]

        [JsonProperty("x")]
        public partial OctoprintApiPrinterAxesAttribute? X { get; set; }

        [ObservableProperty]

        [JsonProperty("y")]
        public partial OctoprintApiPrinterAxesAttribute? Y { get; set; }

        [ObservableProperty]

        [JsonProperty("z")]
        public partial OctoprintApiPrinterAxesAttribute? Z { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
