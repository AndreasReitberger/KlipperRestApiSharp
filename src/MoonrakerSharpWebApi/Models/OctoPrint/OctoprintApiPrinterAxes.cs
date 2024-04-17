using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxes : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("e")]
        OctoprintApiPrinterAxesAttribute? e;

        [ObservableProperty]
        [property: JsonProperty("x")]
        OctoprintApiPrinterAxesAttribute? x;

        [ObservableProperty]
        [property: JsonProperty("y")]
        OctoprintApiPrinterAxesAttribute? y;

        [ObservableProperty]
        [property: JsonProperty("z")]
        OctoprintApiPrinterAxesAttribute? z;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
