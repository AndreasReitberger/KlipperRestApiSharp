using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperPrinterStatusSubscriptionStatus
    {
        #region Properties
        [JsonProperty("objects")]
        public Dictionary<string, string> Objects { get; set; } = new();

        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
