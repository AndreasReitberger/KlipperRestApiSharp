using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusProbe
    {
        #region Properties
        [JsonProperty("last_z_result")]
        public double? LastZResult { get; set; }

        [JsonProperty("last_query")]
        public bool LastQuery { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
