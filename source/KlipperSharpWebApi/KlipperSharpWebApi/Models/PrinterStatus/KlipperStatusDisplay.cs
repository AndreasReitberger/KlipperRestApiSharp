using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusDisplay
    {
        #region Properties
        [JsonProperty("progress")]
        public long Progress { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
