using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperExtruderHistory
    {
        #region Properties
        [JsonProperty("temperatures")]
        public List<double> Temperatures { get; set; } = new();

        [JsonProperty("targets")]
        public List<long> Targets { get; set; } = new();

        [JsonProperty("powers")]
        public List<long> Powers { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString() => JsonConvert.SerializeObject(this, Formatting.Indented);
        #endregion
    }
}
