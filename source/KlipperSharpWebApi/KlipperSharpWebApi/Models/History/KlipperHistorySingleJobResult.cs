using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperHistorySingleJobResult
    {
        #region Properties
        [JsonProperty("job")]
        public KlipperJobItem Job { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
