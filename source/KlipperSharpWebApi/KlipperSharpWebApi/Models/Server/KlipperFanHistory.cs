using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{

    public partial class KlipperFanHistory
    {
        #region Properties
        [JsonProperty("temperatures")]
        public List<double> Temperatures { get; set; } = new();

        [JsonProperty("targets")]
        public List<long> Targets { get; set; } = new();

        [JsonProperty("speeds")]
        public List<long> Speeds { get; set; } = new();
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
