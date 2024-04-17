using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterExtruder
    {
        #region Properties
        [JsonProperty("count")]
        public long Count { get; set; }

        [JsonProperty("nozzleDiameter")]
        public double NozzleDiameter { get; set; }

        [JsonProperty("offsets")]
        public long[][] Offsets { get; set; } = [];

        [JsonProperty("sharedNozzle")]
        public bool SharedNozzle { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
