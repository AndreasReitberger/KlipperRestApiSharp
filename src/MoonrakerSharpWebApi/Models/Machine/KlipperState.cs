using Newtonsoft.Json;

namespace AndreasReitberger.API.Moonraker.Models
{
    public partial class KlipperState
    {
        #region Properties
        [JsonProperty("active_state")]
        public string ActiveState { get; set; }

        [JsonProperty("sub_state")]
        public string SubState { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
