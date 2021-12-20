using Newtonsoft.Json;

namespace AndreasReitberger.Models
{
    public partial class KlipperMachineInfoResult
    {
        #region Properties
        [JsonProperty("system_info")]
        public KlipperMachineInfo SystemInfo { get; set; }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }
}
