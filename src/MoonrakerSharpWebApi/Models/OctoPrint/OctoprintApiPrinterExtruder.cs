using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterExtruder : ObservableObject
    {
        #region Properties
        [ObservableProperty]
        [property: JsonProperty("count")]
        long count;

        [ObservableProperty]
        [property: JsonProperty("nozzleDiameter")]
        double nozzleDiameter;

        [ObservableProperty]
        [property: JsonProperty("offsets")]
        long[][] offsets = [];

        [ObservableProperty]
        [property: JsonProperty("sharedNozzle")]
        bool sharedNozzle;
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
