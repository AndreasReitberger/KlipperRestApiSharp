using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperWebSocketMessage
    {
        #region Properties
        [JsonProperty("jsonrpc")]
        public string Jsonrpc { get; set; } = string.Empty;

        [JsonProperty("method")]
        public string Method { get; set; } = string.Empty;

        [JsonProperty("params")]
        public List<object> Params { get; set; } = [];

        [JsonProperty("id")]
        public long? Id { get; set; }
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
