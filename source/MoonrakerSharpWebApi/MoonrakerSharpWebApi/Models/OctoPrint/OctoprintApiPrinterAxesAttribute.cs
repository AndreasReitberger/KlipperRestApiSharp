using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterAxesAttribute
    {
        #region Properties
        [JsonProperty("inverted")]
        public bool Inverted { get; set; }

        [JsonProperty("speed")]
        public long Speed { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
