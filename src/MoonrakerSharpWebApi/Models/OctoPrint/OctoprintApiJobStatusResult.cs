using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class OctoprintApiJobStatusResult
    {
        #region Properties
        [JsonProperty("job")]
        public OctoprintApiJobResult Job { get; set; }

        [JsonProperty("progress")]
        public OctoprintApiJobInfoProgress Progress { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
