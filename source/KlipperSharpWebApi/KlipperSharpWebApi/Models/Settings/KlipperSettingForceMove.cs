using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperSettingForceMove
    {
        #region Properties
        [JsonProperty("enable_force_move")]
        public bool EnableForceMove { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
