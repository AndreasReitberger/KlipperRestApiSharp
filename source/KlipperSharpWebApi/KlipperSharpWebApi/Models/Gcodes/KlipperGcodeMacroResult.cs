using Newtonsoft.Json;
using System.Collections.Generic;

namespace AndreasReitberger.Models
{
    public partial class KlipperGcodeMacroResult
    {
        #region Properties
        [JsonProperty("status")]
        public Dictionary<string, KlipperGcodeMacro> Status { get; set; }

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
