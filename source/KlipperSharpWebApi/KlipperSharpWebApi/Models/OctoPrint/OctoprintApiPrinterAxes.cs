using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class OctoprintApiPrinterAxes
    {
        #region Properties
        [JsonProperty("e")]
        public OctoprintApiPrinterAxesAttribute E { get; set; }

        [JsonProperty("x")]
        public OctoprintApiPrinterAxesAttribute X { get; set; }

        [JsonProperty("y")]
        public OctoprintApiPrinterAxesAttribute Y { get; set; }

        [JsonProperty("z")]
        public OctoprintApiPrinterAxesAttribute Z { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
