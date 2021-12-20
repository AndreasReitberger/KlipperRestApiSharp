using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperStatusPauseResume
    {
        #region Properties
        [JsonProperty("is_paused")]
        public bool IsPaused { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
