using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiPrinterState
    {
        #region Properties
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; } = string.Empty;

        [JsonProperty("flags", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, bool> Flags { get; set; } = [];
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
