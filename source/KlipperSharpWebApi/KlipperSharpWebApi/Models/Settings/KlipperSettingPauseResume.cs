using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperSettingPauseResume
    {
        #region Properties
        [JsonProperty("recover_velocity")]
        public long RecoverVelocity { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
