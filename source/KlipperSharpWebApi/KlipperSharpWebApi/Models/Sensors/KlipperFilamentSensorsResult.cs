using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperFilamentSensorsResult
    {
        #region Properties
        [JsonProperty("status")]
        public Dictionary<string, KlipperStatusFilamentSensor> Status { get; set; }

        [JsonProperty("eventtime")]
        public double Eventtime { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
