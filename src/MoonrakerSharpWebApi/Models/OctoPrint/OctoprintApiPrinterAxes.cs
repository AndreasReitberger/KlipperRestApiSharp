using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxes : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("e")]
        OctoprintApiPrinterAxesAttribute? e;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("x")]
        OctoprintApiPrinterAxesAttribute? x;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("y")]
        OctoprintApiPrinterAxesAttribute? y;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("z")]
        OctoprintApiPrinterAxesAttribute? z;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
