using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("count")]
        long count;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("nozzleDiameter")]
        double nozzleDiameter;

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("offsets")]
        long[][] offsets = [];

        [ObservableProperty, JsonIgnore]
        [property: JsonProperty("sharedNozzle")]
        bool sharedNozzle;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
